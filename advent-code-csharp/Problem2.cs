#define PART1 // change this to solve "PART1" or "PART2" of problem2

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace advent_code_csharp
{

    class Position
    {
        public int horizontal = 0;
        public int depth = 0;
        public int aim = 0;
    }

    public class Problem2
    {
        static readonly string EXAMPLE_FILE = "../../../exampleProblem2.txt";
        static readonly string INPUT_FILE = "../../../inputProblem2.txt";
        const string INCREASE_HORIZONTAL_INSTRUC = "forward";
        const string INCREASE_DEPTH_INSTRUCT = "down";
        const string DECREASE_DEPTH_INSTRUCT = "up";

        public static void Main(string[] args)
        {
            Stopwatch watch = new();
            watch.Start();
            int SumPosition(string instruction, Position pos, int value) => instruction switch
            {
#if PART1
                INCREASE_HORIZONTAL_INSTRUC => pos.horizontal += value,
                INCREASE_DEPTH_INSTRUCT => pos.depth += value,
                DECREASE_DEPTH_INSTRUCT => pos.depth -= value,
                _ => throw new Exception($"Not expected instruction {instruction}")
#endif
#if PART2
                INCREASE_HORIZONTAL_INSTRUC => ForwardInstructionWithAim(ref pos, value),
                INCREASE_DEPTH_INSTRUCT => pos.aim += value,
                DECREASE_DEPTH_INSTRUCT => pos.aim -= value,
                _ => throw new Exception($"Not expected instruction {instruction}")
#endif
            };

            string[] lines = FileUtils.ReadFile<string>(EXAMPLE_FILE);
            Position pos = new();
            foreach (string line in lines)
            {
                string instruction = FileUtils.GetColumnContent<string>(line, ' ', 0);
                int value = FileUtils.GetColumnContent<int>(line, ' ', 1);
                SumPosition(instruction, pos, value);
            }
            watch.Stop();

#if PART1
            Console.WriteLine($"Final Horizontal pos: {pos.horizontal} and final Depth: {pos.depth}. Multiplication: {pos.horizontal * pos.depth}");
#endif
#if PART2
            Console.WriteLine($"Final Horizontal pos: {pos.horizontal} and final Depth: {pos.depth}. Multiplication: {pos.horizontal * pos.depth}");
#endif
            Console.WriteLine($"Execution time {watch.ElapsedMilliseconds}ms");
        }

        private static int ForwardInstructionWithAim(ref Position pos, int value)
        {
            pos.horizontal += value;
            pos.depth += pos.aim * value;
            return 0;
        }

    }
}
