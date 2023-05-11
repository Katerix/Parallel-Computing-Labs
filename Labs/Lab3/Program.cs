using Lab3;
using System.Diagnostics;

// Знайти кількість непарних елементів в масиві та найбільше непарне число.

public static class Program
{
    public static int N = 1000;
    public static int R = 100;
    public static int _threadAmount = 8;

    static Stopwatch timer = new Stopwatch();

    static void Main(string[] ags)
    {
        Console.WriteLine($"Given an integer array (N = {N}). Count the amount of odd number and a max odd value.\n");

        var numbers = Services.RandomInit(R, N).ToArray();

        Console.WriteLine("1. Single thread solution");

        timer.Start();
        var result = numbers.OddCountAndMaxSingleThread();
        timer.Stop();

        PrintResults(result);

        Console.WriteLine("2. Multithread solution using mutex");

        timer.Restart();
        result = numbers.CountOddAndMaxParallelWithLock(_threadAmount);
        timer.Stop();

        PrintResults(result);

        Console.WriteLine("3. Multithread solution using atomic operations");

        timer.Restart();
        result = numbers.OddCountAndMaxParallelWithAtomic(_threadAmount);
        timer.Stop();

        PrintResults(result);

        Console.WriteLine("4. Multithread solution using parallel LINQ");

        timer.Restart();
        result = numbers.OddCountAndMaxParallelLib();
        timer.Stop();

        PrintResults(result);
    }

    static void PrintResults((int,int) result)
    {
        Console.WriteLine($"Result: \nOdd numbers amount: {result.Item1} Max: {result.Item2}");
        Console.WriteLine($"Took: {timer.ElapsedMilliseconds} ms. \n");
    }
}