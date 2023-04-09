using Lab3;
using System.Diagnostics;

// Знайти кількість непарних елементів в масиві та найбільше непарне число.

public static class Program
{
    public static int N = 100000000;
    public static int R = 100;
    public static int _threadAmount = 8;

    static Stopwatch timer = new Stopwatch();
    static Stopwatch timer2 = new Stopwatch();
    static Stopwatch timer3 = new Stopwatch();
    static Stopwatch timer4 = new Stopwatch();


    static void Main(string[] ags)
    {
        Console.WriteLine($"Given an integer array (N = {N}). Count the amount of odd number and a max odd value.\n");

        var numbers = Services.RandomInit(R, N).ToArray();

        Console.WriteLine("1. Single thread solution");

        timer.Start();
        var result = numbers.OddCountAndMaxSingleThread();
        timer.Stop();

        PrintResults(result, timer);

        Console.WriteLine("2. Multithread solution using mutex");

        timer2.Start();
        result = numbers.CountOddAndMaxParallelWithLock(_threadAmount);
        timer2.Stop();

        PrintResults(result, timer2);

        Console.WriteLine("3. Multithread solution using atomic operations");

        timer3.Start();
        result = numbers.OddCountAndMaxParallelWithAtomic(_threadAmount);
        timer3.Stop();

        PrintResults(result, timer3);

        Console.WriteLine("4. Multithread solution using parallel LINQ");

        timer4.Start();
        result = numbers.OddCountAndMaxParallelLib();
        timer4.Stop();

        PrintResults(result, timer4);
    }

    static void PrintResults((int,int) result, Stopwatch timer)
    {
        Console.WriteLine($"Result: \nOdd numbers amount: {result.Item1} Max: {result.Item2}");
        Console.WriteLine($"Took: {timer.ElapsedMilliseconds} ms. \n");
    }
}