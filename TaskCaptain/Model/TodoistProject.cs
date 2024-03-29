﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Collections.Specialized;

namespace TaskCaptain
{
    /// <summary>
    /// TodoistProject represents the Project objects as they exist in the Todoist infrastructure, along with additional functionality
    /// </summary>
    [JsonObject]
    public class TodoistProject : ICollection<TodoistTask>, IEnumerable, INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region Config

        /// <summary>
        /// _taskList is the private backing list that contains all the tasks
        /// </summary>
        [JsonIgnore]
        private ObservableCollection<TodoistTask> _taskList;

        // These are the private backing fields for the below properties
        long _id;
        int _order;
        int _indent;
        string _name;

        // These fields are used for sorting the project by the corresponding name
        SortTasksById _sortTasksById = new SortTasksById();
        SortTasksByOrder _sortTasksByOrder = new SortTasksByOrder();
        SortTasksByPriority _sortTasksByPriority = new SortTasksByPriority();
        SortTasksByContent _sortTasksByContent = new SortTasksByContent();
        SortTasksByDue _sortTasksByDue = new SortTasksByDue();

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Id is the id of the project, as listed in Todoist
        /// Null if offline
        /// </summary>
        [JsonProperty(nameof(Id))]
        public long Id
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

        internal void ObservableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (TodoistTask task in e.NewItems)
                {
                    Add(task);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (TodoistTask task in e.OldItems)
                {
                    Remove(task);
                }
            }

        }

        /// <summary>
        /// Name is the title of the project
        /// </summary>
        [JsonProperty(nameof(Name))]
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
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Name)));
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }
        /// <summary>
        /// Order is an int signifying where in the list of projects this project goes
        /// Null if offline
        /// </summary>
        [JsonProperty(nameof(Order))]
        public int Order
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
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Order)));
                    _order = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Order)));
                }
            }
        }

        /// <summary>
        /// Indent is what tier this project belongs on
        /// Null if offline
        /// </summary>
        [JsonProperty(nameof(Indent))]
        public int Indent
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
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Indent)));
                    _indent = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Indent)));
                }
            }
        }

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
            _taskList = new ObservableCollection<TodoistTask>();
        }

        /// <summary>
        /// This constructor creates a project and inputs tasks via params
        /// </summary>
        /// <param name="name"></param>
        /// <param name="_taskArray"></param>
        public TodoistProject(string name, params TodoistTask[] taskArray)
        {
            Name = name;
            _taskList = new ObservableCollection<TodoistTask>();

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
        [JsonConstructor]
        public TodoistProject(long id, string name, int order, int indent)
        {
            Id = id;
            Name = name;
            Order = order;
            Indent = indent;
            _taskList = new ObservableCollection<TodoistTask>();
        }

        /// <summary>
        /// This full constructor takes details of online projects, along with a task array parameter.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="order"></param>
        /// <param name="indent"></param>
        /// <param name="taskArray"></param>
        public TodoistProject(long id, string name, int order, int indent, params TodoistTask[] taskArray)
        {
            Id = id;
            Name = name;
            Order = order;
            Indent = indent;
            _taskList = new ObservableCollection<TodoistTask>();

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
            if(null == collection)
            {
                throw new ArgumentNullException("The collection to be added cannot be null.", nameof(collection));
            }

            foreach (TodoistTask task in collection)
            {
                Add(task);
            }
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
/*
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
*/
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
    }
}
