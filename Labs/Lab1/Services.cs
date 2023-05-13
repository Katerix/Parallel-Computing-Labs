
namespace Lab1
{
    public static class Services
    {
        public static int[] CountingSortSingleThread(this int[] numbers, int range)
        {
            int[] frequences = new int[range + 1];

            for (int i = 0; i < numbers.Length; i++)
            {
                frequences[numbers[i]]++;
            }

            int currentIndex = 0;

            for (int i = 0; i < frequences.Length; i++)
            {
                if (frequences[i] == 0)
                    continue;
                else
                {
                    numbers[currentIndex] = i;
                    currentIndex++;
                    frequences[i]--;
                    i--;
                }
            }
            return numbers;
        }
        public static IEnumerable<int> RandomInit(int range, int size)
        {
            var rand = new Random();

            for (int i = 0; i < size; i++)
                yield return rand.Next(range);
        }

        public static (int, double) GetModeAndMedianSingleThread(this int[] numbers, int range)
        {
            int[] frequences = new int[range + 1];

            int mode = 0, maxFrequency = 0, count = numbers.Length;

            GetPartialFrequency(ref numbers, ref frequences, 0, count, ref mode, ref maxFrequency, ref count);

            SortWithFrequencies(ref numbers, ref frequences);

            double median;
            if (numbers.Length % 2 == 1)
                median = numbers[numbers.Length / 2];
            else
                median = (numbers[numbers.Length / 2] + numbers[numbers.Length / 2 - 1]) / 2;

            return (mode, median);
        }
        public static (int, double) GetModeAndMedianParallel(this int[] numbers, int range, int threadAmount = 2)
        {
            Thread[] threads = new Thread[threadAmount];

            int[] frequences = new int[range];

            int count = numbers.Length, mode = 0, maxFrequency = 0;

            for (int i = 0; i < threadAmount; i++)
            {
                int start = count / threadAmount * i;
                int end = count / threadAmount * (i + 1);

                if (i == threadAmount - 1)
                {
                    end += count % threadAmount;
                }

                threads[i] = new Thread(() => GetPartialFrequency(ref numbers, ref frequences,
                            start, end, ref mode, ref maxFrequency, ref count));
                threads[i].Start();
            }

            foreach (var th in threads)
                th.Join();

            SortWithFrequencies(ref numbers, ref frequences);

            double median;
            if (numbers.Length % 2 == 1)
                median = numbers[numbers.Length / 2];
            else
                median = (double)(numbers[numbers.Length / 2] + numbers[numbers.Length / 2 - 1]) / 2;

            return (mode, median);
        }

        private static void GetPartialFrequency(ref int[] numbers, ref int[] frequences, int start, int end, ref int mode, ref int maxFrequency, ref int count)
        {
            for (int i = start; i < end; i++)
            {
                if (i >= count) return;

                if (++frequences[numbers[i]] > maxFrequency)
                {
                    maxFrequency = frequences[numbers[i]];
                    mode = numbers[i];
                }
            }
        }

        private static void SortWithFrequencies(ref int[] numbers, ref int[] frequences)
        {
            int currentIndex = 0;

            for (int i = 0; i < frequences.Length; i++)
            {
                if (frequences[i] == 0)
                    continue;
                else
                {
                    numbers[currentIndex] = i;
                    currentIndex++;
                    frequences[i]--;
                    i--;
                }
            }
        }
    }
}
