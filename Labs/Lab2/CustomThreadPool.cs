
namespace Lab2
{
    public class CustomThreadPool
    {
        public Thread[] workingThreads;
        public readonly int _threadAmount;

        public delegate void WorkerMethod();
        public CustomThreadPool(int threadAmount)
        {
            _threadAmount = threadAmount;

            workingThreads = new Thread[_threadAmount];
        }

        public void InitAndStart(WorkerMethod method)
        {
            for (int i = 0; i < _threadAmount; i++)
            {
                workingThreads[i] = new Thread(() => method());
                workingThreads[i].Name = $"Thread {i + 1}";
                workingThreads[i].Start();
            }
        }

        public void Join()
        {
            foreach (var thr in workingThreads)
            { 
                thr.Join();
            }
        }
    }
}
