using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            SanitizeFile(url);
            string[] lines = File.ReadAllLines(url);
            T[] result = new T[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                result[i] = CastToValue<T>(lines[i]);
            }

            return result;
        }

        private static void SanitizeFile(in string url)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Regex trimBetweenRegex = new Regex(@" +");
            File.WriteAllText(url, trimBetweenRegex.Replace(File.ReadAllText(url), @" "));
            string[] lines = File.ReadAllLines(url);
            string[] auxLines = lines;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith(' '))
                {
                    auxLines[i] = lines[i].Substring(1);
                }
            }
            File.WriteAllLines(url, auxLines);
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

        public static T[] GetColumns<T>(string text, char sep)
        {
            string[] columns = text.Split(sep);
            T[] result = new T[columns.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                result[i] = CastToValue<T>(columns[i]);
            }

            return result;
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
