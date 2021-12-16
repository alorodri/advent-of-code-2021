using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advent_code_csharp
{
    public enum Type
    {
        EXAMPLE, INPUT
    }

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

        public static T CastToValue<T>(string s)
        {
            if (typeof(T) == typeof(int))
            {
                return (T)(object)Convert.ToInt32(s);
            } else if (typeof(T) == typeof(uint))
            {
                return (T)(object)Convert.ToUInt32(s);
            } else if (typeof(T) == typeof(byte))
            {
                return (T)(object)Convert.ToByte(s);
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

        public static string GetFileUrl(Type t, uint n)
        {
            if (t == Type.EXAMPLE)
            {
                return "../../../exampleProblem" + n + ".txt";
            } else if (t == Type.INPUT)
            {
                return "../../../inputProblem" + n + ".txt";
            } else
            {
                return "../../../exampleProblem" + n + ".txt";
            }
        }
    }
}
