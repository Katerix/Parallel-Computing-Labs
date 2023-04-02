using Lab2;
using System.Diagnostics;

// 4 working threads take tasks from the queue (all tasks dure up to 45 secs generally)
// 1 task time - 6-12 secs
public class Program
{
    static Queue<Action> taskQueue = new Queue<Action>();
    static object queueLock = new object();
    static bool queueManagerDone = false;
    static int _timeLimit = 45;
    static int _threadCount = 4;

    public static bool _quit = false;

    public static Stopwatch localQueueTime = new Stopwatch();
    public static List<double> fullQueueTime = new List<double>();

    static void Main(string[] args)
    {
        Console.WriteLine("Press q - to quit \n");

        Thread queueManagerThread = new Thread(QueueManagerMethod);
        queueManagerThread.Start();

        Thread keyboardThread = new Thread(KeyboardThreadMethod);
        keyboardThread.Start();

        CustomThreadPool threadPool = new CustomThreadPool(_threadCount);
        threadPool.InitAndStart(WorkerMethod);

        threadPool.Join();
        keyboardThread.Join();
        queueManagerThread.Join();

        Console.WriteLine("Full queue time: \n");
        Console.WriteLine($"Max: {fullQueueTime.Max()} ticks");
        Console.WriteLine($"Min: {fullQueueTime.Min()} ticks");
    }

    static void KeyboardThreadMethod()
    {
        while (!_quit)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                lock (queueLock)
                {
                    if (key.KeyChar == 'q')
                    {
                        Console.WriteLine("Stopping thread pool...\n");
                        _quit = true;
                    }
                }
            }

            Thread.Sleep(100);
        }
    }


    static void QueueManagerMethod()
    {
        while (!_quit)
        {
            lock (queueLock)
            {
                if (_quit) break;

                if (taskQueue.Count == 0)
                {
                    Console.WriteLine("Queue is empty! It will be filled soon...\n");
                    queueManagerDone = false;
                }

                if (!queueManagerDone && taskQueue.Count == 0)
                {
                    taskQueue.FillQueue(_timeLimit);

                    Console.WriteLine("Queue is full!\n");

                    localQueueTime.Restart();

                    queueManagerDone = true;

                    Monitor.PulseAll(queueLock);
                }

                while (taskQueue.Count > 0 && !_quit)
                {
                    Monitor.Wait(queueLock);
                }
            }
        }
    }

    static void WorkerMethod()
    {
        while (true)
        {
            Action task = null;

            lock (queueLock)
            {
                if (_quit)
                {
                    Monitor.Pulse(queueLock);
                    break;
                }
                if (taskQueue.Count == 0)
                {
                    Monitor.PulseAll(queueLock);
                }
                while (taskQueue.Count == 0 && !queueManagerDone)
                {
                    Monitor.Wait(queueLock);
                }
                if (taskQueue.Count > 0)
                {
                    task = taskQueue.Dequeue();

                    if (localQueueTime.IsRunning)
                    {
                        localQueueTime.Stop();
                        fullQueueTime.Add(localQueueTime.ElapsedTicks);
                    }
                }
            }

            if (task != null)
            {
                task.Invoke();
            }
        }
    }
}
