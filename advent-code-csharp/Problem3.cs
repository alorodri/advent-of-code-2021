using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advent_code_csharp
{
    unsafe public class Problem3
    {
        static readonly byte WHEN_EQUAL_O2 = 1;
        static readonly byte WHEN_EQUAL_CO2 = 0;
        public static void Main(string[] args)
        {
            var AddBitRight = (ref uint n) => { n <<= 1; n += 1; };
            var MoveBit = (ref uint n) => n <<= 1;

            Stopwatch watch = new();
            watch.Start();

            string[] lines = FileUtils.ReadFile<string>(FileUtils.GetFileUrl(Type.INPUT, 3));
            uint totalLines = (uint)lines.Length;
            uint lineLength = (uint)lines[0].Length;
            uint[] count = new uint[lines[0].Length];

            List<string> O2List = new(lines);
            List<string> CO2List = new(lines);

            uint gamma = 0;
            uint epsilon;
            uint O2;
            uint CO2;
            CountMostRepeated(lines, ref count);
            foreach(uint reps in count)
            {
                if (FindMostCommon(totalLines, reps) == 1)
                {
                    AddBitRight(ref gamma);
                } else
                {
                    MoveBit(ref gamma);
                }
            }

            for (uint i = 0; O2List.Count > 1; i++) FilterByCriteria(ref O2List, ref count, ref i, &FindMostCommon);
            O2 = Convert.ToUInt32(O2List[0], 2);

            for (uint i = 0; CO2List.Count > 1; i++) FilterByCriteria(ref CO2List, ref count, ref i, &FindLeastCommon);
            CO2 = Convert.ToUInt32(CO2List[0], 2);

            epsilon = MakeEpsilon(gamma, lineLength);
            uint part1Result = gamma * epsilon;
            uint part2Result = O2 * CO2;
            watch.Stop();

            Console.WriteLine($"Gamma: {Convert.ToString(gamma, 2)}({gamma}), Epsilon: {Convert.ToString(epsilon, 2)}({epsilon})");
            Console.WriteLine($"O2 generator rating: {Convert.ToString(O2, 2)}({O2}), CO2 generator rating: {Convert.ToString(CO2, 2)}({CO2})");
            Console.WriteLine($"Result part 1: {part1Result}");
            Console.WriteLine($"Result part 2: {part2Result}");
            Console.WriteLine($"Execution time {watch.ElapsedMilliseconds}ms");
        }

        private static void ResetCounter(ref uint[] count)
        {
            for (int i = 0; i < count.Length; i++)
            {
                count[i] = 0;
            }
        }

        private static void FilterByCriteria(ref List<string> values, ref uint[] count, ref uint pos, delegate*<uint, uint, byte> commonFunc)
        {
            List<string> auxValues = new(values);
            ResetCounter(ref count);
            CountMostRepeated(values.ToArray(), ref count);
            byte commonValue = commonFunc((uint)values.Count, count[pos]);

            foreach (var value in values)
            {
                string ch = value[(int)pos].ToString();
                if(commonValue != FileUtils.CastToValue<byte>(ch))
                {
                    auxValues.Remove(value);
                }
            }
            values = auxValues;
        }

        private static byte FindMostCommon(uint totalLines, uint reps)
        {
            if (totalLines - reps < totalLines / 2)
            {
                return 1;
            } else if (totalLines - reps == totalLines / 2)
            {
                return WHEN_EQUAL_O2;
            } else
            {
                return 0;
            }
        }

        private static byte FindLeastCommon(uint totalLines, uint reps)
        {
            if (totalLines - reps > totalLines / 2)
            {
                return 1;
            }
            else if (totalLines - reps == totalLines / 2)
            {
                return WHEN_EQUAL_CO2;
            }
            else
            {
                return 0;
            }
        }

        private static uint MakeEpsilon(uint n, uint lineLength)
        {
            for(int i = 0; i < lineLength; i++)
            {
                n = (uint)(n ^ (1 << i));
            }

            return n;
        }

        private static void CountMostRepeated(string[] lines, ref uint[] count)
        {
            foreach (var line in lines)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    string c = line[i].ToString();
                    count[i] += FileUtils.CastToValue<uint>(c);
                }
            }
        }
    }
}
