using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace TaskCaptain.REST
{
    /// <summary>
    /// TodoistClient is the static class for making REST calls against the Todoist Server according to the Net.Http class.
    /// Each method will return an HttpResponseMessage per the HTTP request. Any GETs contain an out parameter with the object.
    /// This class also does the Newtonsoft.Json conversion/deconversion.
    /// All passed HttpClients must contain the proper URI and headers.
    /// </summary>
    public static class TodoistClient
    {
        private static readonly string _badClientErrorString = "The Todoist client was malformatted for Todoist communication. It must be pointed at todoist.com and have an authorization header.";

        public static HttpResponseMessage GetAllProjects(HttpClient todoistClient, out ICollection<TodoistProject> allProjects)
        {
            if (!IsTodoistFormat(todoistClient))
            {
                throw new ArgumentException(_badClientErrorString);
            }

            HttpResponseMessage returnedResponse = todoistClient.GetAsync(new Uri(todoistClient.BaseAddress + "/projects")).Result;
            if (returnedResponse.IsSuccessStatusCode)
            {
                string result = returnedResponse.Content.ReadAsStringAsync().Result;
                allProjects = JsonConvert.DeserializeObject<ICollection<TodoistProject>>(result);
            }
            else
            {
                allProjects = null;
            }

            return returnedResponse;
        }

        //public static HttpResponseMessage CreateNewProject(HttpClient todoistClient, string projectName, out TodoistProject newProject)
        //{
        //    if (!IsTodoistFormat(todoistClient))
        //    {
        //        throw new ArgumentException(_badClientErrorString);
        //    }

        //}

        //public static HttpResponseMessage GetProject(HttpClient todoistClient, int projectId, out TodoistProject gotProject)
        //{
        //    if (!IsTodoistFormat(todoistClient))
        //    {
        //        throw new ArgumentException(_badClientErrorString);
        //    }

        //}

        //public static HttpResponseMessage UpdateProject(HttpClient todoistClient, string projectName)
        //{
        //    if (!IsTodoistFormat(todoistClient))
        //    {
        //        throw new ArgumentException(_badClientErrorString);
        //    }

        //}

        //public static HttpResponseMessage DeleteProject(HttpClient todoistClient, int projectId)
        //{
        //    if (!IsTodoistFormat(todoistClient))
        //    {
        //        throw new ArgumentException(_badClientErrorString);
        //    }

        //}

        public static HttpResponseMessage GetAllActiveTasks(HttpClient todoistClient, out ICollection<TodoistTask> allTasks)
        {
            if (!IsTodoistFormat(todoistClient))
            {
                throw new ArgumentException(_badClientErrorString);
            }

            HttpResponseMessage returnedResponse = todoistClient.GetAsync(new Uri(todoistClient.BaseAddress + "/tasks")).Result;
            if (returnedResponse.IsSuccessStatusCode)
            {
                string result = returnedResponse.Content.ReadAsStringAsync().Result;
                allTasks = JsonConvert.DeserializeObject<ICollection<TodoistTask>>(result);
            }
            else
            {
                allTasks = null;
            }

            return returnedResponse;

        }

        //public static HttpResponseMessage GetAllActiveTasks(HttpClient todoistClient, int projectId, out ICollection<TodoistTask> projectTasks)
        //{
        //    if (!IsTodoistFormat(todoistClient))
        //    {
        //        throw new ArgumentException(_badClientErrorString);
        //    }

        //}

        //public static HttpResponseMessage GetAllActiveTasks(HttpClient todoistClient, string filter, out ICollection<TodoistTask> filterTasks)
        //{
        //    if (!IsTodoistFormat(todoistClient))
        //    {
        //        throw new ArgumentException(_badClientErrorString);
        //    }

        //}

        //public static HttpResponseMessage GetAllActiveTasks(HttpClient todoistClient, int projectId, string filter, out ICollection<TodoistTask> filterTasks)
        //{
        //    if (!IsTodoistFormat(todoistClient))
        //    {
        //        throw new ArgumentException(_badClientErrorString);
        //    }

        //}

        //public static HttpResponseMessage CreateNewTask(HttpClient todoistClient, string content, out TodoistTask newTask)
        //{
        //    if (!IsTodoistFormat(todoistClient))
        //    {
        //        throw new ArgumentException(_badClientErrorString);
        //    }

        //}

        //public static HttpResponseMessage CreateNewTask(HttpClient todoistClient, string content, int projectId, int order, int priority, TodoistDue due, out TodoistTask newTask)
        //{
        //    if (!IsTodoistFormat(todoistClient))
        //    {
        //        throw new ArgumentException(_badClientErrorString);
        //    }

        //}

        public static HttpResponseMessage GetActiveTask(HttpClient todoistClient, int taskId, out TodoistTask gotTask)
        {
            if (!IsTodoistFormat(todoistClient))
            {
                throw new ArgumentException(_badClientErrorString);
            }

            HttpResponseMessage returnedResponse = todoistClient.GetAsync(new Uri(todoistClient.BaseAddress + "/tasks/" + taskId)).Result;
            if (returnedResponse.IsSuccessStatusCode)
            {
                string result = returnedResponse.Content.ReadAsStringAsync().Result;
                gotTask = JsonConvert.DeserializeObject<TodoistTask>(result);
            }
            else
            {
                gotTask = null;
            }

            return returnedResponse;

        }

        //public static HttpResponseMessage UpdateTaskContent(HttpClient todoistClient, int taskId, string newContent)
        //{
        //    if (!IsTodoistFormat(todoistClient))
        //    {
        //        throw new ArgumentException(_badClientErrorString);
        //    }

        //}

        //public static HttpResponseMessage UpdateTaskProject(HttpClient todoistClient, int taskId, int newProjectId)
        //{
        //    if (!IsTodoistFormat(todoistClient))
        //    {
        //        throw new ArgumentException(_badClientErrorString);
        //    }

        //}

        //public static HttpResponseMessage UpdateTaskPriority(HttpClient todoistClient, int taskId, int newPriority)
        //{
        //    if (!IsTodoistFormat(todoistClient))
        //    {
        //        throw new ArgumentException(_badClientErrorString);
        //    }

        //}

        //public static HttpResponseMessage UpdateTaskDue(HttpClient todoistClient, int taskId, TodoistDue newDue)
        //{
        //    if (!IsTodoistFormat(todoistClient))
        //    {
        //        throw new ArgumentException(_badClientErrorString);
        //    }

        //}

        ///// <summary>
        ///// The command does exactly what official clients do when you close a task. Regular tasks are completed and moved to history, subtasks are checked (marked as done, but not moved to history), recurring tasks are moved forward (due date is updated).
        ///// </summary>
        ///// <param name="todoistClient"></param>
        ///// <param name="taskId"></param>
        ///// <returns></returns>
        //public static HttpResponseMessage CloseTask(HttpClient todoistClient, int taskId)
        //{
        //    if (!IsTodoistFormat(todoistClient))
        //    {
        //        throw new ArgumentException(_badClientErrorString);
        //    }

        //}

        ///// <summary>
        ///// This command reopens a previously closed task. Works both with checked tasks in the user’s workspace and tasks moved to history. The behaviour varies for different types of tasks (the command follows the behaviour of official clients when tasks are uncompleted or extracted from the history).
        ///// Regular tasks are extracted from the history and added back to the user workspace as normal unchecked tasks(without their subtasks though).
        ///// Completed subtasks of a non-completed task are simply marked as uncompleted.
        ///// Subtasks that were moved to history are added back to the workspace as first-level tasks.
        ///// Non-completed recurring tasks are ignored.
        ///// </summary>
        ///// <param name="todoistClient"></param>
        ///// <param name="taskId"></param>
        ///// <returns></returns>
        //public static HttpResponseMessage ReopenTask(HttpClient todoistClient, int taskId)
        //{
        //    if (!IsTodoistFormat(todoistClient))
        //    {
        //        throw new ArgumentException(_badClientErrorString);
        //    }

        //}

        //public static HttpResponseMessage DeleteTask(HttpClient todoistClient, int taskId)
        //{
        //    if (!IsTodoistFormat(todoistClient))
        //    {
        //        throw new ArgumentException(_badClientErrorString);
        //    }

        //}

        private static bool IsTodoistFormat(HttpClient todoistClient)
        {
            if (null == todoistClient)
            {
                return false;
            }

            if (null == todoistClient.BaseAddress)
            {
                todoistClient.BaseAddress =  new Uri("https://beta.todoist.com/API/v8");
            }

            if (!todoistClient.BaseAddress.IsWellFormedOriginalString() &&
               !todoistClient.BaseAddress.ToString().Contains("todoist.com") &&
               !todoistClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                return false;
            }

            return true;
        }
    }
}
