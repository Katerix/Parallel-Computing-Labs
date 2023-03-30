using Lab2;

// 4 working threads take tasks from the queue (all tasks dure up to 45 secs generally)
// 1 task time - 6-12 secs
public class Program 
{
    static void Main(string[] ags) 
    {
        CustomThreadPool threadPool = new CustomThreadPool();
        
        while (true) 
        {
            if (!threadPool.taskQueue.Any())
            {
                Console.WriteLine("No more tasks in the queue. Let's add them again. \n");

                lock (threadPool.taskQueue)
                {
                    threadPool.FillQueue();
                }

                threadPool.InitThreads();
            }
        }
    }
}