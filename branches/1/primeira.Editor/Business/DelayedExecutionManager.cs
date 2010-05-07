using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace primeira.Editor
{
    public static class DelayedExecutionManager
    {
        private class TaskItem
        {
            public DateTime ExecutionTime { get; set; }

            public Action TaskMethod { get; set; }

            public Object ObjectState { get; set; }

            public bool Active { get; set; }
        }

        private static List<TaskItem> _taks = new List<TaskItem>();

        private static Timer _timer;

        public static void AddTask(object objectState, int delay, Action taskMethod)
        {
            if (_timer == null)
            {
                TimerCallback c = new TimerCallback(ProcessTasks);

                _timer = new Timer(c, null, 100, 100);

            }

            TaskItem t = new TaskItem();
            t.ExecutionTime = DateTime.Now.AddMilliseconds(delay);
            t.TaskMethod = taskMethod;
            t.ObjectState = objectState;
            t.Active = true;

            _taks.Add(t);
        }

        private static void ProcessTasks(object obj)
        {
            _taks.AsParallel().ForAll(x =>
            {
                if (x.Active && x.ExecutionTime < DateTime.Now)
                {
                    x.TaskMethod.Invoke();
                    x.Active = false;
                }
            });

            _taks.RemoveAll(x => !x.Active);
        }

        public static void AbortTask(Object objectState)
        {
            _taks.AsParallel().ForAll(x =>
            {
                if (x.ObjectState == objectState)
                {
                    x.Active = false;
                }
            });
        }

    }
}
