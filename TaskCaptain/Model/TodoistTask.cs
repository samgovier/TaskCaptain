using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace TaskCaptain
{
    /// <summary>
    /// TodoistTask represents the Task objects as they exist in the Todoist infrastructure, along with additional functionality
    /// </summary>
    [JsonObject]
    public class TodoistTask : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Config

        // These are the private backing fields for the below properties
        long? _id;
        long? _projectId;
        int? _order;
        int? _indent;
        int _priority;
        string _content;
        string _webUrl;

        /// <summary>
        /// Id is the id of the task, as listed in Todoist
        /// Null if offline
        /// </summary>
        [JsonProperty(nameof(Id))]
        public long? Id
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
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Id)));
                    _id = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
                }
            }
        }

        /// <summary>
        /// ProjectId is the id of the project this task is assigned to. MUST match with an existing TodoistProject.Id
        /// Null if offline
        /// </summary>
        [JsonProperty("project_id")]
        public long? ProjectId
        {
            get
            {
                return _projectId;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Project Id cannot be a negative number.");
                }
                else
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(ProjectId)));
                    _projectId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProjectId)));
                }
            }
        }


        /// <summary>
        /// Content is the actual string content of the task: what's gotta be done?
        /// </summary>
        [JsonProperty(nameof(Content))]
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Task content cannot be empty.");
                }
                else
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Content)));
                    _content = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Content)));
                }
            }
        }

        /// <summary>
        /// Order is an int signifying where in the project the task is ordered
        /// Null if offline
        /// </summary>
        [JsonProperty(nameof(Order))]
        public int? Order
        {
            get
            {
                return _order;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Order cannot be a negative number.");
                }
                else
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Order)));
                    _order = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Order)));
                }
            }
        }

        /// <summary>
        /// Indent is what tier the task belongs on
        /// Null if offline
        /// </summary
        [JsonProperty(nameof(Indent))]
        public int? Indent
        {
            get
            {
                return _indent;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Indent cannot be a negative number.");
                }
                else
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Indent)));
                    _indent = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Indent)));
                }
            }
        }


        /// <summary>
        /// Priority is 1-4, the Todoist priority of the task
        /// </summary>
        [JsonProperty(nameof(Priority))]
        public int Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Priority cannot be a negative number.");
                }
                else
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Priority)));
                    _priority = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Priority)));
                }
            }
        }


        /// <summary>
        /// Due is when the project is due to be completed. Can be null if there's no due date.
        /// </summary>
        [JsonProperty(nameof(Due))]
        public TodoistDue Due { get; set; }

        /// <summary>
        /// WebUrl is the direct url to access the task in the Todoist web API
        /// </summary>
        [JsonProperty("url")]
        public string WebUrl
        {
            get
            {
                return _webUrl;
            }
            set
            {
                _webUrl = value;
            }

        }

        /// <summary>
        /// IsCompleted is a boolean to state whether the task is completed or not
        /// </summary>
        [JsonProperty("completed")]
        public bool IsCompleted { get; set; }

        /// <summary>
        /// PropertyChanging is called when a property is in the process of being changed
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// PropertyChanged is called when a property has finished changing
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// This simple constructor just creates a default task, given the content
        /// </summary>
        /// <param name="content"></param>
        public TodoistTask(string content)
        {
            Content = content;
            IsCompleted = false;
            Priority = 4;
        }

        /// <summary>
        /// Test constructor
        /// </summary>
        public TodoistTask()
        {
            // NO
            Due = new TodoistDue();
        }

        /// <summary>
        /// This constructor creates a task with a little more detail
        /// </summary>
        /// <param name="content"></param>
        /// <param name="projectId"></param>
        /// <param name="priority"></param>
        /// <param name="due"></param>
        public TodoistTask(string content, long? projectId, int priority, TodoistDue due)
        {
            Content = content;
            ProjectId = projectId;
            Priority = priority;
            Due = due;
            IsCompleted = false;
        }

        /// <summary>
        /// This full constructor takes all details from an online task and creates the TodoistTask object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectId"></param>
        /// <param name="content"></param>
        /// <param name="completed"></param>
        /// <param name="order"></param>
        /// <param name="indent"></param>
        /// <param name="priority"></param>
        /// <param name="due"></param>
        /// <param name="webUrl"></param>
        [JsonConstructor]
        public TodoistTask(long id, long projectId, string content, bool completed, int order, int indent, int priority, TodoistDue due, string webUrl)
        {
            Id = id;
            ProjectId = projectId;
            Content = content;
            IsCompleted = completed;
            Order = order;
            Indent = indent;
            Priority = priority;
            Due = due;
            WebUrl = webUrl;
        }
        #endregion
        #region Functions

        public void MoveToProject(TodoistProject destProject)
        {
            ProjectId = destProject.Id;
        }

        #endregion
    }
}
