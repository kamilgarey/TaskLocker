using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskLockerSpeedMeasure
{
    class Program
    {
        static void Main(string[] args)
        {
            RunTwoTimes(EmptyTaskSpeedComparision);
            Console.Read();
        }

        private static async void RunTwoTimes(Func<Task> theAction)
        {
            // jit compilation at first
            await theAction();
            // pure runtime
            await theAction();

        }

        private void AnAttemptToImprovePerformance(Task task)
        {
            task.Start();
        }

        private static async Task EmptyTaskSpeedComparision()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            Task task1 = new Task(() => { });
            task1.Start();
            await task1;
            sw.Stop();
            Console.WriteLine("Elapsed={0}", sw.Elapsed);

            Task task2 = new Task(() => { });

            ThreadPool.UnsafeQueueCustomWorkItem(task2, false);
            await task1;

        }
    }
}
