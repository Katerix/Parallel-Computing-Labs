
namespace Lab2
{
    public class TaskQueue
    {
        public Queue<Task> taskQueue;
        public int _queueTimeLimit;

        public TaskQueue()
        {
            taskQueue = new Queue<Task>();
            _queueTimeLimit = 45;
        }

        public void FillQueue()
        {
            int summaryTime = 0;

            while (summaryTime <= _queueTimeLimit)
            {
                var timeForCurrentTask = GetRandomExecutionTime();

                summaryTime += timeForCurrentTask;

                if (summaryTime > _queueTimeLimit) break;

                taskQueue.Enqueue(new Task(() => Work(timeForCurrentTask)));
            }
        }

        public static int GetRandomExecutionTime() => 6 + new Random().Next() % 6;

        public void Work(int time)
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} started to execute task ({time}s)...\n");

            Thread.Sleep(time * 1000);

            Console.WriteLine($"{Thread.CurrentThread.Name} finished task ({time}s) execution.\n");
        }

        public void StartQueue()
        {
            while (true)
            {
                if (!taskQueue.Any())
                {
                    lock (taskQueue)
                    {
                        Thread.Sleep(1000);

                        Console.WriteLine("Queue is currently empty! New tasks will be added soon! \n");

                        FillQueue();

                        Console.WriteLine("Queue has been filled! \n");
                    }
                } 
            }
        }
    }
}
