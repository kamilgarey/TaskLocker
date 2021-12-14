using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskLocker
{
    public class TaskLocker : IDisposable
    {
        private readonly object _syncRoot = new object();

        private readonly Queue<Task> _waitingQueue = new Queue<Task>();

        private bool _isLocked = false;

        private void DoNoting()
        {

        }

        public Task Enter()
        {
            lock (_syncRoot)
            {
                if (_isLocked)
                {
                    Task waitingOne = new Task(DoNoting);
                    _waitingQueue.Enqueue(waitingOne);
                    return waitingOne;
                }

                _isLocked = true;
                return Task.CompletedTask;
            }
        }

        public void Leave()
        {
            lock (_syncRoot)
            {
                if (_waitingQueue.Count == 0)
                {
                    _isLocked = false;
                }
                else
                {
                    Task oneGotLock = _waitingQueue.Dequeue();
                    oneGotLock.Start();
                }
            }
        }

        public void Dispose()
        {
            Leave();
        }
    }
}