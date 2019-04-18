﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskCaptain
{
    internal class SortTasksById : IComparer<TodoistTask>
    {
        public int Compare(TodoistTask x, TodoistTask y)
        {
            if(!(x.IsOnline) || !(y.IsOnline))
            {
                throw new TodoistOfflineException();
            }
            else
            {
                int xId = (int)x.Id;
                int yId = (int)y.Id;
                return xId.CompareTo(yId);
            }
        }
    }

    internal class SortTasksByOrder : IComparer<TodoistTask>
    {
        public int Compare(TodoistTask x, TodoistTask y)
        {
            if (!(x.IsOnline) || !(y.IsOnline))
            {
                throw new TodoistOfflineException();
            }
            else
            {
                int xOrder = (int)x.Order;
                int yOrder = (int)y.Order;
                return xOrder.CompareTo(yOrder);
            }
        }
    }

    internal class SortTasksByPriority : IComparer<TodoistTask>
    {
        public int Compare(TodoistTask x, TodoistTask y)
        {
            return x.Priority.CompareTo(y.Priority);
        }

    }

    public class SortProjectsByName : IComparer<TodoistProject>
    {
        public int Compare(TodoistProject x, TodoistProject y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }

    public class SortProjectsById : IComparer<TodoistProject>
    {
        public int Compare(TodoistProject x, TodoistProject y)
        {
            if (!(x.IsOnline) || !(y.IsOnline))
            {
                throw new TodoistOfflineException();
            }
            else
            {
                int xId = (int)x.Id;
                int yId = (int)y.Id;
                return xId.CompareTo(yId);
            }
        }
    }

    public class SortProjectsByOrder : IComparer<TodoistProject>
    {
        public int Compare(TodoistProject x, TodoistProject y)
        {
            if (!(x.IsOnline) || !(y.IsOnline))
            {
                throw new TodoistOfflineException();
            }
            else
            {
                int xOrder = (int)x.Order;
                int yOrder = (int)y.Order;
                return xOrder.CompareTo(yOrder);
            }
        }
    }
}