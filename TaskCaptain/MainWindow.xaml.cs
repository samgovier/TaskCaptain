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
using Microsoft.VisualBasic;

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

        public MainWindow()
        {
            InitializeTodoistAccount();
            InitializeComponent();
            FocusGrid.ItemsSource = _todoistAcct[1];
        }

        private void InitializeTodoistAccount()
        {
            string restApiToken = "ede60c0039f7ed477d38bc1f15f762ac615fe8c8";
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
    }
}
