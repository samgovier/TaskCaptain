using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskCaptain;

namespace Boatswain
{
    /// <summary>
    /// Boatswain is coordinator, testing the data structures and basic functionality of the Todoist activities
    /// </summary>
    [TestClass]
    public class Boatswain
    {
        static TodoistTask testTask1Online = new TodoistTask(3, 1, "Hello", false, 1, 1, 4, null, null);
        static TodoistTask testTask2Online = new TodoistTask(1, 1, "How are you", false, 2, 1, 4, null, null);
        static TodoistTask testTask3Online = new TodoistTask(2, 1, "Goodbye", false, 3, 1, 4, null, null);
        static TodoistProject testProjectOnline;
        static TodoistProject testProjectOffline;
        static TodoistTask testTask1Offline = new TodoistTask("TestTask");
        static TodoistTask testTask2Offline = new TodoistTask("TestTask", 2, 4, new TodoistDue(DateTime.Today, false));
        static TodoistTask testTask3Offline = new TodoistTask("TestTask1", null, 1, null);

        [TestMethod]
        public void CreateProject()
        {
            testProjectOnline = new TodoistProject(3, "Test", 1, 1, testTask1Online, testTask2Online, testTask3Online);
            testProjectOffline = new TodoistProject("TestAgain");
        }

        [TestMethod]
        public void AddRemoveToProject()
        {
            testProjectOffline.Add(testTask1Offline);
            testProjectOffline.Add(testTask2Offline);
            testProjectOffline.Add(testTask3Offline);
            //testProjectOnline.Remove(testTask3Online);
        }

        [TestMethod]
        public void EnumerateProject()
        {
           int i = 0;
           foreach (TodoistTask task in testProjectOnline)
           {
               i += (int)task.Order;
           }
           Assert.AreEqual(6, i);

           string j = "";
           foreach (TodoistTask task in testProjectOffline)
           {
               j += task.Content;
           }
           Assert.AreEqual("TestTaskTestTaskTestTask1", j);
        }

        [TestMethod]
        public void CopyToFunctionality()
        {
            TodoistTask[] taskArray = new TodoistTask[3];
            testProjectOnline.CopyTo(taskArray, 0);
        }

        [TestMethod]
        public void SortingFunctionality()
        {
            //testProjectOffline.SortByPriority();
            //testProjectOnline.SortByPriority();
            //testProjectOnline.SortByOrder();
            //testProjectOnline.SortById();
        }

        [TestMethod]
        public void ModifyTask()
        {
           testProjectOnline[0].Content = "Who are you";
        }

        [TestMethod]
        public void CompleteTask()
        {
           testProjectOnline[1].MarkComplete();
        }

        [TestMethod]
        public void TodoistDueConstructors()
        {
            TodoistDue testDue = new TodoistDue("tom", "2019-04-20", null, false, null);
        }
    }
}
