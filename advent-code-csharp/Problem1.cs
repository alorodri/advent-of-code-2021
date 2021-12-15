using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advent_code_csharp
{
    public class Problem1
    {
        static readonly string EXAMPLE_FILE = "../../../exampleProblem1.txt";
        static readonly string INPUT_FILE = "../../../inputProblem1.txt";
        static readonly int INCREMENTS = 3;
        public static void Main(string[] args)
        {
            Stopwatch watch = new();
            watch.Start();
            int[] lines = Int32CastStringArray(File.ReadAllLines(INPUT_FILE));
            int totalIncrements = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (i != 0 && i + INCREMENTS <= lines.Length && SumIncrements(lines, i) > SumIncrements(lines, i-1)) totalIncrements++;
            }
            watch.Stop();
            Console.WriteLine(totalIncrements);
            Console.WriteLine($"Execution time {watch.ElapsedMilliseconds}ms");
        }

        private static int SumIncrements(int[] arr, int position)
        {
            int sum = 0;
            for (int i = 0; i < INCREMENTS; i++)
            {
                sum += arr[position + i];
            }

            return sum;
        }

        private static int[] Int32CastStringArray(string[] arr)
        {
            int[] arrcpy = new int[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                arrcpy[i] = Convert.ToInt32(arr[i]);
            }

            return arrcpy;
        }
    }
}
