
namespace Lab2
{
    public class MyQueue // concurrent
    {
        public Queue<Task> taskQueue;
        public int _queueTimeLimit;

        public MyQueue()
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
        // q put & get synchronized 
        // wait notify - monitor
        
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
