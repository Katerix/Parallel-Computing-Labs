using System.Threading;

namespace Lab3
{
    public static class Services
    {
        static object _lock = new object();

        public static IEnumerable<int> RandomInit(int range, int size)
        {
            var rand = new Random();

            for (int i = 0; i < size; i++)
                yield return rand.Next(range);
        }
        public static int[] RandomInitUnique(int size)
        {
            List<int> numbers = new List<int>();
            Random random = new Random();

            while (numbers.Count < size)
            {
                int num = random.Next();
                if (!numbers.Contains(num))
                {
                    numbers.Add(num);
                }
            }

            return numbers.ToArray();
        }

        public static (int, int) OddCountAndMaxSingleThread(this int[] numbers)
        {
            int oddCount = 0, oddMax = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] % 2 != 0)
                {
                    oddCount++;

                    if (numbers[i] > oddMax) oddMax = numbers[i];
                }
            }

            return (oddCount, oddMax);
        }
        public static (int, int) CountOddAndMaxParallelWithLock(this int[] numbers, int threadsAmount)
        {
            int size = numbers.Length;
            int chunk = size / threadsAmount;

            int oddCount = 0, oddMax = 0;

            Thread[] threads = new Thread[threadsAmount];

            for (int i = 0; i < threadsAmount; i++)
            {
                int start = i * chunk;
                int end = i == threadsAmount - 1 ? size : start + chunk;

                threads[i] = new Thread(() => CountOddAndMaxSubmethod(numbers, start, end, ref oddCount, ref oddMax));
            }

            foreach (var thread in threads) thread.Start();

            foreach (var thread in threads) thread.Join();

            return (oddCount, oddMax);
        }
        private static void CountOddAndMaxSubmethod(int[] numbers, int start, int end, ref int oddCount, ref int oddMax)
        {
            for (int i = start; i < end; i++)
            {
                if (numbers[i] % 2 != 0)
                {
                    lock (_lock)
                    {
                        oddCount++;

                        if (numbers[i] > oddMax)
                        {
                            oddMax = numbers[i];
                        }
                    }
                }
            }
        }
        public static (int, int) OddCountAndMaxParallelWithAtomic(this int[] numbers, int threadsAmount)
        {
            int oddCount = 0, oddMax = 0;

            Parallel.ForEach(numbers, (number) =>
            {
                if (number % 2 != 0)
                {
                    Interlocked.Increment(ref oddCount);

                    if (number > oddMax)
                    {
                        Interlocked.Exchange(ref oddMax, number);
                    }
                }
            });

            return (oddCount, oddMax);
        }
        public static (int, int) OddCountAndMaxParallelLib(this IEnumerable<int> numbers)
        {
            var odds = numbers.AsParallel().Where(n => n % 2 != 0);

            return (odds.AsParallel().Count(), odds.AsParallel().Max());
        }

    }
}
