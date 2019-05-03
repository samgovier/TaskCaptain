using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TaskCaptain
{
    /// <summary>
    /// TodoistProject represents the Project objects as they exist in the Todoist infrastructure, along with additional functionality
    /// </summary>
    public class TodoistProject : ICollection<TodoistTask>, IEnumerable
    {
        #region Config

        /// <summary>
        /// _taskList is the private backing list that contains all the tasks
        /// </summary>
        private List<TodoistTask> _taskList;

        // These are the private backing fields for the below properties
        int? _id;
        int? _order;
        int? _indent;
        string _name;

        // These fields are used for sorting the project by the corresponding name
        SortTasksById _sortTasksById = new SortTasksById();
        SortTasksByOrder _sortTasksByOrder = new SortTasksByOrder();
        SortTasksByPriority _sortTasksByPriority = new SortTasksByPriority();
        SortTasksByContent _sortTasksByContent = new SortTasksByContent();
        SortTasksByDue _sortTasksByDue = new SortTasksByDue();

        /// <summary>
        /// Id is the id of the project, as listed in Todoist
        /// Null if offline
        /// </summary>
        public int? Id
        {
            get
            {
                return _id;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Id cannot be a negative number.");
                }
                else
                {
                    _id = value;
                }
            }
        }

        /// <summary>
        /// Name is the title of the project
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Task content cannot be empty.");
                }
                else
                {
                    _name = value;
                }
            }
        }
        /// <summary>
        /// Order is an int signifying where in the list of projects this project goes
        /// Null if offline
        /// </summary>
        public int? Order
        {
            get
            {
                return _order;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Order cannot be a negative number.");
                }
                else
                {
                    _order = value;
                }
            }
        }

        /// <summary>
        /// Indent is what tier this project belongs on
        /// Null if offline
        /// </summary>
        public int? Indent
        {
            get
            {
                return _indent;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Indent cannot be a negative number.");
                }
                else
                {
                    _indent = value;
                }
            }
        }

        /// <summary>
        /// IsOnline is a boolean marking whether or not this task is online, and other values are expected
        /// </summary>
        public bool IsOnline { get; private set; }

        /// <summary>
        /// IsReadOnly is a boolean marking whether or not this item is editable
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Count is the amount of tasks that are in the project
        /// </summary>
        public int Count
        {
            get
            {
                return _taskList.Count;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// This simple constructor just creates a simple project, given the name
        /// </summary>
        /// <param name="name"></param>
        public TodoistProject(string name)
        {
            Name = name;
            _taskList = new List<TodoistTask>();
            IsOnline = false;
        }

        /// <summary>
        /// This constructor creates a project and inputs tasks via params
        /// </summary>
        /// <param name="name"></param>
        /// <param name="_taskArray"></param>
        public TodoistProject(string name, params TodoistTask[] taskArray)
        {
            Name = name;
            _taskList = new List<TodoistTask>();
            IsOnline = false;

            foreach (TodoistTask task in taskArray)
            {
                _taskList.Add(task);
            }
        }

        /// <summary>
        /// This full constructor takes all details from an online project and creates the TodoistTask object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="order"></param>
        /// <param name="indent"></param>
        public TodoistProject(int id, string name, int order, int indent)
        {
            Id = id;
            Name = name;
            Order = order;
            Indent = indent;
            _taskList = new List<TodoistTask>();
            IsOnline = true;
        }

        /// <summary>
        /// This full constructor takes details of online projects, along with a task array parameter.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="order"></param>
        /// <param name="indent"></param>
        /// <param name="taskArray"></param>
        public TodoistProject(int id, string name, int order, int indent, params TodoistTask[] taskArray)
        {
            Id = id;
            Name = name;
            Order = order;
            Indent = indent;
            _taskList = new List<TodoistTask>();
            IsOnline = true;

            foreach (TodoistTask task in taskArray)
            {
                _taskList.Add(task);
            }
        }

        #endregion

        #region Standard Functions

        public void Add(TodoistTask toAdd)
        {
            _taskList.Add(toAdd);
        }

        public void AddRange(IEnumerable<TodoistTask> collection)
        {
            _taskList.AddRange(collection);
        }

        public void Clear()
        {
            _taskList.Clear();
        }

        public bool Contains(TodoistTask toCheck)
        {
            return _taskList.Contains(toCheck);
        }

        public void CopyTo(TodoistTask[] outputArray, int arrayIndex)
        {
            _taskList.CopyTo(outputArray, arrayIndex);
        }

        public bool Remove(TodoistTask toRemove)
        {
            return _taskList.Remove(toRemove);
        }

        public void SortById()
        {
            _taskList.Sort(_sortTasksById);
        }

        public void SortByOrder()
        {
            _taskList.Sort(_sortTasksByOrder);
        }

        public void SortByPriority()
        {
            _taskList.Sort(_sortTasksByPriority);
        }

        public void SortByContent()
        {
            _taskList.Sort(_sortTasksByContent);
        }

        public void SortByDue()
        {
            _taskList.Sort(_sortTasksByDue);
        }

        public IEnumerator GetEnumerator()
        {
            return _taskList.GetEnumerator();
        }

        IEnumerator<TodoistTask> IEnumerable<TodoistTask>.GetEnumerator()
        {
            return _taskList.GetEnumerator();
        }

        public TodoistTask this[int index]
        {
            get
            {
                return _taskList[index];
            }
        }

        #endregion

        #region Automated Functions

        /// <summary>
        /// Create tasks on the last workday of each month.
        /// </summary>
        public static void CreateLastWorkdayTasks(string subject, int priority, TodoistProject destProject)
        {
            TodoistTask[] tasksToAdd = new TodoistTask[12];
            for (int i = 0; i < tasksToAdd.Length; i++)
            {
                int monthDelta = DateTime.Today.Month + i;
                int currentMonth = monthDelta % 12;
                int currentYear = DateTime.Today.Year + (int)Math.Floor((double)monthDelta / 12);
                int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);
                DateTime taskDate = new DateTime(currentYear, currentMonth, lastDay);
                do
                {
                    if (TodoistDue.IsWeekend(taskDate) || TodoistDue.IsHoliday(taskDate))
                    {
                        taskDate = taskDate.AddDays(-1.0d);
                    }
                    else
                    {
                        TodoistDue taskDueDate = new TodoistDue(taskDate, false);
                        tasksToAdd[i] = new TodoistTask(subject, destProject.Id, priority, taskDueDate);
                    }
                } while (null == tasksToAdd[i]);
            }

            destProject.AddRange(tasksToAdd);
        }

        /// <summary>
        /// Move all tasks passed via parameter to either Monday or Sunday, depending on if they're weekday or weekend
        /// </summary>
        public static void ScheduleToWeekStart(params TodoistTask[] tasksToMove)
        {
            foreach (TodoistTask task in tasksToMove)
            {
                DateTime taskDate;

                if (task.Due.TryParseToDateTime(out taskDate))
                {
                    if (taskDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        task.Due = new TodoistDue(taskDate.AddDays(-1.0d), task.Due.Recurring);
                    }
                    else if (DayOfWeek.Tuesday <= taskDate.DayOfWeek && taskDate.DayOfWeek <= DayOfWeek.Friday)
                    {
                        task.Due = new TodoistDue(taskDate.AddDays(DayOfWeek.Monday - taskDate.DayOfWeek), task.Due.Recurring);
                    }
                }
            }
        }

        /// <summary>
        /// Move all tasks from an old project to an incoming project (like the inbox) to be re-sorted
        /// </summary>
        public static void ClearToProject(TodoistProject incomingProject, TodoistProject oldProject)
        {
            if(!incomingProject.IsOnline || !oldProject.IsOnline)
            {
                throw new TodoistOfflineException("Projects must be online to be cleared to.");
            }
            else
            {
                foreach(TodoistTask task in oldProject)
                {
                    incomingProject.Add(task);
                    oldProject.Remove(task);
                    // REST ACTION
                }
            }
        }

        /// <summary>
        /// Translate a task into it's recurrences for a specified amount of days
        /// </summary>
        public static void TranslateRecurrence(TodoistTask recurringTask, TimeSpan interval)
        {
            if (!recurringTask.Due.Recurring)
            {
                return;
            }
            
            recurringTask.Due.TryParseToDateTime(out DateTime initialDue);
            DateTime currentDue = initialDue;
            while (interval > currentDue.Subtract(initialDue))
            {
                // PARSE RECURRING STATUS
            }
        }

        #endregion
    }
}
