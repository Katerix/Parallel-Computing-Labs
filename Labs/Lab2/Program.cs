using Lab2;
using System;
using System.Diagnostics;


// 4 working threads take tasks from the queue (all tasks dure up to 45 secs generally)
public class Program 
{
    static void Main(string[] ags) 
    {
        CustomThreadPool threadPool = new CustomThreadPool();

        /*
         while (true) 
        {
            if (!threadPool.taskQueue.Any())
            {
                threadPool.FillQueue();
            }


        }
        */
    }
}