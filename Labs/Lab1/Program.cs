using Lab1;
using System.Diagnostics;

//given big array => create second array to use it for counting sort => extract mode and median
//so i need counting sort for parallel and single thread computing
public class Program
{
    public const int N = 100000; //quantity
    public const int M = 15; //range
    public const int A = 8; //thread amount

    static void Main(string[] arg)
    {
        var stopwatch = new Stopwatch();

        int[] array = Services.RandomInit(M, N).ToArray();
        int[] array2 = array;

        stopwatch.Start();
        var result = array.GetModeAndMedianSingleThread(M);
        stopwatch.Stop();

        Console.WriteLine($"Single thread execution. Thread amount is: 1");
        Console.WriteLine($"Size is: {N}");
        Console.WriteLine($"Range is: 0 - {M - 1}");
        Console.WriteLine($"Mode is: {result.Item1}");
        Console.WriteLine($"Median is: {result.Item2}");
        Console.WriteLine($"Time is: {stopwatch.ElapsedMilliseconds} ms.");
        Console.WriteLine();

        stopwatch = new Stopwatch();

        stopwatch.Start();
        var result2 = array2.GetModeAndMedianParallel(M, A);
        stopwatch.Stop();

        Console.WriteLine($"Parallel execution. Thread amount is: {A}");
        Console.WriteLine($"Size is: {N}");
        Console.WriteLine($"Range is: 0 - {M - 1}");
        Console.WriteLine($"Mode is: {result2.Item1}");
        Console.WriteLine($"Median is: {result2.Item2}");
        Console.WriteLine($"Time is: {stopwatch.ElapsedMilliseconds} ms.");
        Console.WriteLine();

        if (result.Item1 == result2.Item1 && result.Item2 == result2.Item2) Console.WriteLine("Success!");
    }
}
