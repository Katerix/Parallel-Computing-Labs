using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class CustomThreadPool
    {
        private Thread[] workingThreads;
        public Queue<Task> taskQueue;
        public int _workingThreadAmount;
        public int _queueTimeLimit;
        public int _taskCounter;

        public CustomThreadPool() 
        {
            _workingThreadAmount = 4;
            _queueTimeLimit = 45; // seconds
            _taskCounter = 0;
            taskQueue = new Queue<Task>();
            workingThreads = new Thread[_workingThreadAmount];
            FillQueue();
            InitThreads();
        }

        public void FillQueue()
        {
            int summaryTime = 0;

            while (summaryTime <= _queueTimeLimit)
            {
                var timeForCurrentTask = GetRandomExecutionTime();

                summaryTime += timeForCurrentTask;

                if (summaryTime > 45) break;

                _taskCounter++;

                taskQueue.Enqueue(new Task(() => Work(timeForCurrentTask, _taskCounter)));
            }
        }

        public void InitThreads()
        {
            for (int i = 0; i < _workingThreadAmount; i++)
            {
                workingThreads[i] = new Thread(() =>
                {
                    while (true)
                    {
                        Task task;

                        lock (taskQueue)
                        {
                            if (taskQueue.Count == 0) break;

                            task = taskQueue.Dequeue();
                        }

                        task.RunSynchronously();
                    }
                });

                workingThreads[i].Name = $"Thread {i + 1}";
                workingThreads[i].Start();
            }

            foreach (var thread in workingThreads)
                thread.Join();
        }

        private static int GetRandomExecutionTime() => 6 + new Random().Next() % 6;

        private void Work(int time, int count)
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} started to execute task ({time}s)...\n");

            Thread.Sleep(time);

            Console.WriteLine($"{Thread.CurrentThread.Name} finished task ({time}s) execution.\n");
        }
    }
}
