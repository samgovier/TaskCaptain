using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace TaskCaptain.REST
{
    public static class TodoistServer
    {
        public static HttpResponseMessage GetAllProjects(HttpClient todoistClient, out ICollection<TodoistProject> allProjects)
        {

        }

        public static HttpResponseMessage CreateNewProject(HttpClient todoistClient, string projectName, out TodoistProject newProject)
        {

        }

        public static HttpResponseMessage GetProject(HttpClient todoistClient, int projectId, out TodoistProject gotProject)
        {

        }

        public static HttpResponseMessage UpdateProject(HttpClient todoistClient, string projectName)
        {

        }

        public static HttpResponseMessage DeleteProject(HttpClient todoistClient, int projectId)
        {

        }

        public static HttpResponseMessage GetActiveTasks(HttpClient todoistClient, out ICollection<TodoistTask> allTasks)
        {

        }

        public static HttpResponseMessage GetActiveTasks(HttpClient todoistClient, int projectId, out ICollection<TodoistTask> projectTasks)
        {

        }

        public static HttpResponseMessage GetActiveTasks(HttpClient todoistClient, string filter, out ICollection<TodoistTask> filterTasks)
        {

        }

        public static HttpResponseMessage GetActiveTasks(HttpClient todoistClient, int projectId, string filter, out ICollection<TodoistTask> filterTasks)
        {

        }

        public static HttpResponseMessage CreateNewTask(HttpClient todoistClient, string content, out TodoistTask newTask)
        {

        }

        public static HttpResponseMessage CreateNewTask(HttpClient todoistClient, string content, int projectId, int order, int priority, TodoistDue due, out TodoistTask newTask)
        {

        }
    }
}
