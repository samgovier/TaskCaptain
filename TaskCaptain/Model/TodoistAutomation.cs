using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;

namespace TaskCaptain
{
    /// <summary>
    /// TodoistAutomation contains the static automation functions of the TaskCaptain application
    /// </summary>
    public static class TodoistAutomation
    {
        #region Automated Functions

        /// <summary>
        /// Create tasks on the last workday of each month.
        /// </summary>
        public static void CreateLastWorkdayTasks(string subject, int priority, TodoistProject destProject, int monthSpan)
        {
            TodoistTask[] tasksToAdd = new TodoistTask[monthSpan];
            for (int i = 0; i < tasksToAdd.Length; i++)
            {
                // currentLoc is the current year and time we're getting a date from
                DateTime currentLoc = DateTime.Today.AddMonths(i);
                
                // taskDate pulls the almost correct date: next month day one, minus one
                DateTime taskDate = (new DateTime(currentLoc.Year, currentLoc.Month + 1, 1)).AddDays(-1);
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