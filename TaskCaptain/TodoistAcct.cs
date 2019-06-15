using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskCaptain
{
    class TodoistAcct : ICollection<TodoistProject>, IEnumerable
    {
        #region Config

        /// <summary>
        /// _projectList is the private backing list that contains all projects from an account
        /// </summary>
        private List<TodoistProject> _projectList;
        
        /// <summary>
        /// ApiToken is the API string used to communicate with the Todoist servers
        /// </summary>
        public string ApiToken { get; private set; }

        // These fields are used for sorting projects by Id, Name, and Order
        SortProjectsById _sortProjectsById = new SortProjectsById();
        SortProjectsByName _sortProjectsByName = new SortProjectsByName();
        SortProjectsByOrder _sortProjectsByOrder = new SortProjectsByOrder();

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
            _projectList = new List<TodoistProject>();

            foreach (TodoistProject project in projectArray)
            {
                _projectList.Add(project);
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

        #endregion
    }
}