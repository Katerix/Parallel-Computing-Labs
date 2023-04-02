
namespace Lab2
{
    public static class Services
    {
        public static Random _random = new Random();

        public static void FillQueue(this Queue<Action> taskQueue, int timeLimit)
        {
            int summaryTime = 0;

            while (summaryTime <= timeLimit)
            {
                var timeForCurrentTask = GetRandomExecutionTime();

                summaryTime += timeForCurrentTask;

                if (summaryTime > timeLimit) break;

                taskQueue.Enqueue(() => Work(timeForCurrentTask));
            }
        }

        public static int GetRandomExecutionTime() => _random.Next(6, 12);

        public static void Work(int time)
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} started to execute task ({time}s)...\n");

            Thread.Sleep(time * 1000);

            Console.WriteLine($"{Thread.CurrentThread.Name} finished task ({time}s) execution.\n");
        }
    }
}
