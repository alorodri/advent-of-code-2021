using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advent_code_csharp
{
    internal class FileUtils
    {
        public static T[] ReadFile<T>(in string url)
        {
            string[] lines = File.ReadAllLines(url);
            T[] result = new T[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                result[i] = CastToValue<T>(lines[i]);
            }

            return result;
        }

        private static T CastToValue<T>(string s)
        {
            if (typeof(T) == typeof(int))
            {
                return (T)(object)Convert.ToInt32(s);
            } else if (typeof(T) == typeof(string))
            {
                return (T)(object)s;
            } else
            {
                Console.WriteLine($"I don't know how to parse value {s}");
                return default;
            }
        }

        public static T GetColumnContent<T>(string text, char sep, uint column)
        {
            string[] contentSp = text.Split(sep);
            return CastToValue<T>(contentSp[column]);
        }
    }
}
