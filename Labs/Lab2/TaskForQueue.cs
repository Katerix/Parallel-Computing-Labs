using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class TaskForQueue
    {
        public Task task { get; set; }

        public TaskForQueue() => task = new Task(() => Work(GetRandomExecutionTime()));

        public static int GetRandomExecutionTime() => 6 + new Random().Next() % 6;

        public void Work(int time)
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} started to execute task ({time}s)...\n");

            Thread.Sleep(time * 1000);

            Console.WriteLine($"{Thread.CurrentThread.Name} finished task ({time}s) execution.\n");
        }

    }
}
