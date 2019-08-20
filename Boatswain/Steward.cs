using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskCaptain;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using TaskCaptain.REST;

namespace Boatswain
{
    /// <summary>
    /// Steward is the class specifically for testing the REST web request API, and communications with Todoist itself
    /// </summary>
    [TestClass]
    public class Steward
    {
        [TestMethod]
        public void GetPersonalProjects()
        {
            HttpClient testclient = new HttpClient();

            testclient.DefaultRequestHeaders.Add("Authorization", "Bearer ");

            string response = testclient.GetStringAsync(new Uri("https://beta.todoist.com/API/v8/projects")).Result;
            //string responseSingle = testclient.GetStringAsync(new Uri("https://beta.todoist.com/API/v8/projects/180783822")).Result;
            //TodoistProject inputSingle = JsonConvert.DeserializeObject<TodoistProject>(responseSingle);
            List<TodoistProject> input = JsonConvert.DeserializeObject<List<TodoistProject>>(response);

            Assert.AreEqual(input[0].Name, "Inbox");
        }

        [TestMethod]
        public void GetPersonalTasks()
        {
            HttpClient testclient = new HttpClient();

            testclient.DefaultRequestHeaders.Add("Authorization", "Bearer ");

            string response = testclient.GetStringAsync(new Uri("https://beta.todoist.com/API/v8/tasks")).Result;
            //string responseSingle = testclient.GetStringAsync(new Uri("https://beta.todoist.com/API/v8/tasks/3178098427")).Result;

            //var dict = Newtonsoft.Json.Linq.JObject.Parse(responseSingle);
            List<TodoistTask> input = JsonConvert.DeserializeObject<List<TodoistTask>>(response);
            //TodoistTask inputSingle = JsonConvert.DeserializeObject<TodoistTask>(responseSingle);

            Assert.AreEqual(input[0].IsCompleted, false);
        }

        [TestMethod]
        public void TestProjectsAgain()
        {
            TodoistProject testProjectOnline = new TodoistProject(3, "Test", 1, 1);

            string testy = JsonConvert.SerializeObject(testProjectOnline);
            TodoistProject newBoi = JsonConvert.DeserializeObject<TodoistProject>(testy);

        }

        [TestMethod]
        public void TestCreateAndDeleteNewProject()
        {
            HttpClient testclient = new HttpClient();
            testclient.DefaultRequestHeaders.Add("Authorization", "Bearer 4019f27f4a31859906535ff630dcac7ebb541062");
            HttpResponseMessage response = TodoistClient.CreateNewProject(testclient, "TEST PROJ", out TodoistProject newbaby);
            Assert.AreEqual("TEST PROJ", newbaby.Name);
            TodoistClient.DeleteProject(testclient, newbaby.Id);
        }
    }
}