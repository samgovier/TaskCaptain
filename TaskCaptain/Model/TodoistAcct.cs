﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Specialized;

namespace TaskCaptain
{
    class TodoistAcct : ICollection<TodoistProject>, IEnumerable, INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region Config

        /// <summary>
        /// _projectList is the private backing list that contains all projects from an account
        /// </summary>
        private ObservableCollection<TodoistProject> _projectList;
        
        /// <summary>
        /// ApiToken is the API string used to communicate with the Todoist servers
        /// </summary>
        public string ApiToken { get; private set; }

        // These fields are used for sorting projects by Id, Name, and Order
        SortProjectsById _sortProjectsById = new SortProjectsById();
        SortProjectsByName _sortProjectsByName = new SortProjectsByName();
        SortProjectsByOrder _sortProjectsByOrder = new SortProjectsByOrder();

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Count is the amount of Projects in the account
        /// </summary>
        public int Count
        {
            get
            {
                return _projectList.Count;
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

        #endregion

        #region Constructors

        /// <summary>
        /// This constructor creates the account with project inputs
        /// </summary>
        /// <param name="apiToken"></param>
        /// <param name="projectArray"></param>
        public TodoistAcct(string apiToken, params TodoistProject[] projectArray)
        {
            ApiToken = apiToken;
            _projectList = new ObservableCollection<TodoistProject>();

            foreach (TodoistProject project in projectArray)
            {
                _projectList.Add(project);
            }
        }

        internal void ObservableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
           if(e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach(TodoistTask task in e.NewItems)
                {
                    GetProjectById(task.ProjectId).Add(task);
                }
            }
           else if(e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach(TodoistTask task in e.OldItems)
                {
                    GetProjectById(task.ProjectId).Remove(task);
                }
            }
          
        }

        #endregion

        #region Standard Functions

        public void Add(TodoistProject toAdd)
        {
            _projectList.Add(toAdd);
        }

        public void Clear()
        {
            _projectList.Clear();
        }

        public bool Contains(TodoistProject toCheck)
        {
            return _projectList.Contains(toCheck);
        }

        public void CopyTo(TodoistProject[] outputArray, int arrayIndex)
        {
            _projectList.CopyTo(outputArray, arrayIndex);
        }

        public bool Remove(TodoistProject toRemove)
        {
            return _projectList.Remove(toRemove);
        }
/*
        public void SortById()
        {
            _projectList.Sort(_sortProjectsById);
        }

        public void SortByOrder()
        {
            _projectList.Sort(_sortProjectsByOrder);
        }

        public void SortByName()
        {
            _projectList.Sort(_sortProjectsByName);
        }
*/
        public IEnumerator GetEnumerator()
        {
            return _projectList.GetEnumerator();
        }

        IEnumerator<TodoistProject> IEnumerable<TodoistProject>.GetEnumerator()
        {
            return _projectList.GetEnumerator();
        }

        public TodoistProject this[int index]
        {
            get
            {
                return _projectList[index];
            }
        }

        public TodoistProject GetProjectById(long projId)
        {
            foreach(TodoistProject project in _projectList)
            {
                if(projId == project.Id)
                {
                    return project;
                }
            }

            return null;
        }

        public TodoistTask[] GetTasksForDates(params DateTime[] selectedDates)
        {
            List<TodoistTask> dateTasks = new List<TodoistTask>();

            foreach(TodoistProject project in _projectList)
            {
                foreach(TodoistTask task in project)
                {
                    if (null != task.Due)
                    {
                        task.Due.TryParseToDateTime(out DateTime taskDT);

                        foreach(DateTime date in selectedDates)
                        {
                            if (selectedDates.Equals(taskDT))
                            {
                                dateTasks.Add(task);
                                break;
                            }
                        }
                    }
                }
            }

            return dateTasks.ToArray();
        }

        public ObservableCollection<TodoistTask> GetObservableTasks(TodoistProject sourceProject)
        {
            return new ObservableCollection<TodoistTask>(sourceProject.ToList());
        }

        public ObservableCollection<TodoistTask> GetObservableTasks(params DateTime[] forDates)
        {
            return new ObservableCollection<TodoistTask>(GetTasksForDates(forDates));
        }

        public ObservableCollection<TodoistProject> GetObservableProjects()
        {
            return new ObservableCollection<TodoistProject>(_projectList.ToList());
        }

        #endregion
    }
}