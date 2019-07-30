using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace TaskCaptain
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TodoistAcct _todoistAcct;
        private HttpClient _todoistClient;
        private string _todoistEndpoint = "https://beta.todoist.com/API/v8";
        private ObservableCollection<TodoistTask> _filterGridView;
        private List<DateTime> _filterGridModel;
        private ObservableCollection<TodoistTask> _projectGridView;
        private TodoistProject _projectGridModel;

        public MainWindow()
        {
            InitializeTodoistAccount();
            InitializeComponent();
            InitializeTwoWayGrids();
            FocusGrid.ItemsSource = _todoistAcct.GetObservableTasks(_todoistAcct[1]);
            RecurEnumerateGrid.ItemsSource = DateRngSchGrid.ItemsSource = _todoistAcct.GetObservableTasks(_todoistAcct[1]);
            LastWkPrjCombo.ItemsSource = BacktoInbPrjCombo.ItemsSource = _todoistAcct.GetObservableProjects();

            ProjectTreeView.ItemsSource = _todoistAcct;
        }

        private void InitializeTwoWayGrids()
        {
            _filterGridView = _todoistAcct.GetObservableTasks(_todoistAcct[1]);
            _projectGridModel = _todoistAcct[1];
            FilterGrid.ItemsSource = _filterGridView;
            _projectGridView = _todoistAcct.GetObservableTasks(_todoistAcct[1]);
            _filterGridModel = null;
            ProjectGrid.ItemsSource = _projectGridView;
            _projectGridView.CollectionChanged += _projectGridModel.ObservableCollectionChanged;
            _filterGridView.CollectionChanged += _todoistAcct.ObservableCollectionChanged;
        }

        private void InitializeTodoistAccount()
        {
            string restApiToken = "4019f27f4a31859906535ff630dcac7ebb541062";
            _todoistClient = new HttpClient();
            _todoistClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + restApiToken);
            string getProjectsString = _todoistClient.GetStringAsync(new Uri(_todoistEndpoint + "/projects")).Result;
            string getTasksString = _todoistClient.GetStringAsync(new Uri(_todoistEndpoint + "/tasks")).Result;
            List<TodoistProject> getProjectsList = JsonConvert.DeserializeObject<List<TodoistProject>>(getProjectsString);
            List<TodoistTask> getTasksList = JsonConvert.DeserializeObject<List<TodoistTask>>(getTasksString);

            //place each task in it's corresponding project
            foreach(TodoistTask taskItem in getTasksList)
            {
                foreach(TodoistProject projectItem in getProjectsList)
                {
                    if(projectItem.Id == taskItem.ProjectId)
                    {
                        projectItem.Add(taskItem);
                        break;
                    }
                }
            }

            _todoistAcct = new TodoistAcct(restApiToken, getProjectsList.ToArray());
        }

        private void SyncTodoistAccount()
        {
            string getProjectsString = _todoistClient.GetStringAsync(new Uri(_todoistEndpoint + "/projects")).Result;
            string getTasksString = _todoistClient.GetStringAsync(new Uri(_todoistEndpoint + "/tasks")).Result;
            List<TodoistProject> getProjectsList = JsonConvert.DeserializeObject<List<TodoistProject>>(getProjectsString);
            List<TodoistTask> getTasksList = JsonConvert.DeserializeObject<List<TodoistTask>>(getTasksString);

            //place each task in it's corresponding project
            foreach (TodoistTask taskItem in getTasksList)
            {
                foreach (TodoistProject projectItem in getProjectsList)
                {
                    if (projectItem.Id == taskItem.ProjectId)
                    {
                        projectItem.Add(taskItem);
                        break;
                    }
                }
            }

            _todoistAcct = new TodoistAcct(_todoistAcct.ApiToken, getProjectsList.ToArray());
        }

        private void SyncButton_Click(object sender, RoutedEventArgs e)
        {
            SyncTodoistAccount();
        }

        private void LastWkRunAutomation_Click(object sender, RoutedEventArgs e)
        {
            TodoistAutomation.CreateLastWorkdayTasks(LastWkDescBox.Text, Convert.ToInt32(LastWkPriCombo.SelectedItem.ToString().Split(' ')[1]), (TodoistProject)LastWkPrjCombo.SelectedItem, Convert.ToInt32(LastWkRngBox.Text));
        }

        private void DateRngSchTasks_Click(object sender, RoutedEventArgs e)
        {
            List<TodoistTask> tasksToMove = new List<TodoistTask>();

            foreach (DateTime selectedDate in DateRngSchCal.SelectedDates)
            {
                tasksToMove.AddRange(_todoistAcct.GetTasksForDates(selectedDate));
            }

            DateRngSchGrid.ItemsSource = tasksToMove;
        }

        private void DateRngSchRunAutomation_Click(object sender, RoutedEventArgs e)
        {
            TodoistTask[] tasksToMove = new TodoistTask[DateRngSchGrid.SelectedItems.Count];
            DateRngSchGrid.SelectedItems.CopyTo(tasksToMove, 0);
            TodoistAutomation.ScheduleToWeekStart(tasksToMove);
        }

        private void BackToInbRunAutomation_Click(object sender, RoutedEventArgs e)
        {
            TodoistAutomation.ClearToProject(_todoistAcct[0], (TodoistProject)BacktoInbPrjCombo.SelectedItem);
        }

        private void RecurEnumerateTasks_Click(object sender, RoutedEventArgs e)
        {
            List<TodoistTask> tasksToDisplay = new List<TodoistTask>();

            foreach (TodoistProject prj in _todoistAcct)
            {
                foreach(TodoistTask tsk in prj)
                {
                    if ((null != tsk.Due) && tsk.Due.Recurring)
                    {
                        tasksToDisplay.Add(tsk);
                    }
                }
            }

            RecurEnumerateGrid.ItemsSource = tasksToDisplay;
        }

        private void RecurEnumerateRunAutomation_Click(object sender, RoutedEventArgs e)
        {
            foreach(TodoistTask recurringTask in RecurEnumerateGrid.SelectedItems)
            {
                TodoistAutomation.TranslateRecurrence(recurringTask, new TimeSpan());
            }
        }
    }
}
