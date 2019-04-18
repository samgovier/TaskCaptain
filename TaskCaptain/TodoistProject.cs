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

        public void Clear()
        {
            _taskList.Clear();
        }

        public bool Contains(TodoistTask toCheck)
        {
            return _taskList.Contains(toCheck);
        }

        public void CopyTo (TodoistTask[] outputArray, int arrayIndex)
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
        public void CreateLastWorkdayTasks(string subject, int priority)
        {
            TodoistTask[] tasksToAdd = new TodoistTask[12];
            for (int i = 0; i < tasksToAdd.Length; i++)
            {
                int currentMonth = (DateTime.Today.Month + i) % 12;
                int currentYear = DateTime.Today.Year + (int)Math.Floor((double)i / 12);
                int lastDay = DateTime.DaysInMonth(currentYear, currentMonth);
                DateTime taskDate = new DateTime(currentYear, currentMonth, lastDay);
                while (null == tasksToAdd[i])
                {
                    if (TodoistDue.IsWeekend(taskDate) || TodoistDue.IsHoliday(taskDate))
                    {
                        taskDate = taskDate.AddDays(-1.0d);
                    }
                    else
                    {
                        TodoistDue taskDueDate = new TodoistDue(taskDate);
                        TodoistTask taskToAdd = new TodoistTask(subject, Id, priority, taskDueDate);
                        tasksToAdd[i] = taskToAdd;
                    }
                }
            }
        }

        #endregion
    }
}
