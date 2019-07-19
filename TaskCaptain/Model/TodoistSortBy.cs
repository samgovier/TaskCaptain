using System;
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
                long xId = (long)x.Id;
                long yId = (long)y.Id;
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

    internal class SortTasksByContent : IComparer<TodoistTask>
    {
        public int Compare(TodoistTask x, TodoistTask y)
        {
            return x.Content.CompareTo(y.Content);
        }

    }

    internal class SortTasksByDue : IComparer<TodoistTask>
    {
        public int Compare(TodoistTask x, TodoistTask y)
        {
            return x.Due.CompareTo(y.Due);
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
                long xId = (long)x.Id;
                long yId = (long)y.Id;
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
