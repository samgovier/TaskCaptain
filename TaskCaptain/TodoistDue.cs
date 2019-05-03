using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskCaptain
{
    public class TodoistDue : IComparable
    {
        #region Config
        private static DateTime[] _holidayList = HolidayList(DateTime.Now.Year);

        /// <summary>
        /// EntryString is the human defined date in arbitrary format
        /// </summary>
        public string EntryString { get; private set; }
        
        /// <summary>
        /// DaveValue is the date in format YYYY-MM-DD corrected to timezone.
        /// </summary>
        public string DateValue { get; private set; }

        /// <summary>
        /// DateTimeValue is the date and time in UTC
        /// null if not set
        /// </summary>
        public string DateTimeValue { get; private set; }

        /// <summary>
        /// Recurring is whether or not the task is a recurring task
        /// </summary>
        public bool Recurring { get; private set; }

        /// <summary>
        /// User's timezone definition
        /// null if DateTimeValue isn't set
        /// </summary>
        public string TimeZone { get; private set; }

        #endregion

        #region Constructors
        /// <summary>
        /// This full constructor takes all the details from an online Due object and creates the TodoistDue object
        /// </summary>
        /// <param name="_entryString"></param>
        /// <param name="_date"></param>
        /// <param name="_dateTime"></param>
        /// <param name="_recurring"></param>
        /// <param name="_timeZone"></param>
        public TodoistDue(string _entryString, string _date, string _dateTime, bool _recurring, string _timeZone)
        {
            EntryString = _entryString;
            DateValue = _date;
            DateTimeValue = _dateTime;
            Recurring = _recurring;
            TimeZone = _timeZone;
        }

        /// <summary>
        /// This DateTime constructor uses a DateTime object to create a Due object
        /// </summary>
        /// <param name="taskDate"></param>
        /// <param name="_recurring"></param>
        public TodoistDue(DateTime taskDate, bool _recurring)
        {
            EntryString = DateValue = taskDate.ToString("yyyy-MM-dd");

            if (taskDate.TimeOfDay.ToString() != "00:00:00")
            {
                EntryString = DateTimeValue = DateValue + "T" + taskDate.ToUniversalTime().TimeOfDay.ToString() + "Z";
                TimeZone = TimeZoneInfo.Local.StandardName;
            }

            Recurring = _recurring;
        }
        #endregion

        #region Functions

        public bool TryParseToDateTime(out DateTime parsedDateTime)
        {
            if(!DateTime.TryParse(DateTimeValue, out parsedDateTime))
            {
                if(!DateTime.TryParse(DateValue, out parsedDateTime))
                {
                    return false;
                }
            }

            return true;
        }

        public int CompareTo(TodoistDue value)
        {
            this.TryParseToDateTime(out DateTime xDateTime);
            value.TryParseToDateTime(out DateTime yDateTime);
            return xDateTime.CompareTo(yDateTime);
        }

        public int CompareTo(object obj)
        {
            if (obj.GetType() == typeof(TodoistDue))
            {
                return CompareTo(obj as TodoistDue);
            }
            else
            {
                this.TryParseToDateTime(out DateTime xDateTime);
                return xDateTime.CompareTo(obj);
            }
        }
        #endregion

        #region Static Properties
        internal static bool IsWeekend(DateTime taskDate)
        {
            if (taskDate.DayOfWeek == DayOfWeek.Saturday || taskDate.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static bool IsHoliday(DateTime taskDate)
        {
            DateTime[] holidays = HolidayList(taskDate.Year);
            foreach (DateTime holiday in holidays)
            {
                if (taskDate == holiday)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Static Functions

        private static DateTime[] HolidayList(int year)
        {
            // 1st of January
            DateTime newYearsDay = new DateTime(year, 1, 1);
            // Last Monday of May
            DateTime memorialDay = LastDayOfMonth(DayOfWeek.Monday, 5, year);
            // 4th of July
            DateTime independenceDay = new DateTime(year, 7, 4);
            // First Monday of September
            DateTime laborDay = NthDayOfMonth(1, DayOfWeek.Monday, 9, year);
            // Fourth Thursday of November
            DateTime thanksgiving = NthDayOfMonth(4, DayOfWeek.Thursday, 11, year);
            // 25th of December
            DateTime christmasDay = new DateTime(year, 12, 25);
            return new DateTime[] { newYearsDay, memorialDay, independenceDay, laborDay, thanksgiving, christmasDay };
        }

        private static DateTime NthDayOfMonth(int n, DayOfWeek day, int month, int year)
        {
            DateTime dayOfMonth = new DateTime(year, month, 1);
            while (true)
            {
                if (dayOfMonth.DayOfWeek == day)
                {
                    return dayOfMonth.AddDays(7 * (n - 1));
                }
                else
                {
                    dayOfMonth = dayOfMonth.AddDays(1);
                }
            }
        }

        private static DateTime LastDayOfMonth(DayOfWeek day, int month, int year)
        {
            DateTime lastDay = NthDayOfMonth(4, day, month, year);
            if (DateTime.DaysInMonth(year, month) - lastDay.Day >= 7)
            {
                lastDay = lastDay.AddDays(7);
            }
            return lastDay;

        }

        #endregion
    }
}
