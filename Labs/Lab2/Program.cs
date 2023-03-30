using Lab2;

// 4 working threads take tasks from the queue (all tasks dure up to 45 secs generally)
// 1 task time - 6-12 secs
public class Program 
{
    static void Main(string[] ags) 
    {
        CustomThreadPool threadPool = new CustomThreadPool();
        MyQueue mainQueue = new MyQueue();

        Thread queueManager = new Thread(() => mainQueue.StartQueue());

        threadPool.InitThreads(mainQueue.taskQueue);

        queueManager.Start();

        //queueManager.Join();

        threadPool.StartThreads();
    }
}
