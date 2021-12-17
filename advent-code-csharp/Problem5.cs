using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// NOT FINISHED

namespace advent_code_csharp
{
    class Line
    {
        public Point start;
        public Point end;
        public bool painted = false;
        public Line(Point start, Point end)
        {
            this.start = start;
            this.end = end;
        }
        public override string ToString() => String.Format($"({start.x},{start.y}) -> ({end.x}, {end.y})");
    }

    class Point
    {
        public int x;
        public int y;
        public int counter = 0;
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static Point ParsePoint(in string str)
        {
            int x = int.Parse(str.Split(',')[0]);
            int y = int.Parse(str.Split(',')[1]);
            Point p = new Point(x, y);
            return p;
        }
        public override string ToString() => String.Format($"({x}, {y})");
    }

    public class Problem5
    {
        static Func<int, int, int, int> FindGreatest = (int a, int b, int greatest) =>
        {
            if (a > greatest) {
                greatest = a;
            } 

            if (b > greatest) {
                greatest = b;
            }

            return greatest;
        };

        public static void Main(string[] args)
        {
            int greatestX = 0;
            int greatestY = 0;
            string[] linesFromFile = FileUtils.ReadFile<string>(FileUtils.GetFileUrl(Type.EXAMPLE, 5));
            List<Line> lines = new List<Line>();
            Dictionary<string, Point> points = new();
            foreach (var lineFromFile in linesFromFile)
            {
                string[] pointsFromFile = lineFromFile.Split(" -> ");
                Line line = new(Point.ParsePoint(pointsFromFile[0]), Point.ParsePoint(pointsFromFile[1]));
                greatestX = FindGreatest(line.start.x, line.end.x, greatestX);
                greatestY = FindGreatest(line.start.y, line.end.y, greatestY);
                lines.Add(line);
            }

            foreach (Line line in lines)
            {
                if (line.painted) continue;
                for (int x = line.start.x; x <= line.end.x; x++)
                {
                    for (int y = line.start.y; y <= line.end.y; y++)
                    {
                        Point p = new(x, y);
                        if (points.ContainsKey(p.ToString()))
                        {
                            points[p.ToString()].counter++;
                        } else
                        {
                            points.Add(p.ToString(), p);
                        }
                        line.painted = true;
                    }
                }
            }

            foreach (Line line in lines)
            {
                if (line.painted) continue;
                for (int x = line.end.x; x >= line.start.x; x--)
                {
                    for (int y = line.end.y; y >= line.start.y; y--)
                    {
                        Point p = new(x, y);
                        if (points.ContainsKey(p.ToString()))
                        {
                            points[p.ToString()].counter++;
                        }
                        else
                        {
                            points.Add(p.ToString(), p);
                        }
                        line.painted = true;
                    }
                }
            }

            int result = 0;
            for (int x = 0; x < greatestX; x++)
            {
                for (int y = 0; y < greatestY; y++)
                {
                    string pointKey = String.Format($"({x}, {y})");
                    string print = points.ContainsKey(pointKey) && points[pointKey].counter > 0 ? Convert.ToString(points[pointKey].counter) : ".";
                    Console.Write($"{print}");
                    if (!points.ContainsKey(pointKey)) continue;
                    if (points[pointKey].counter > 1) result++;
                }
                Console.WriteLine();
            }

            Console.WriteLine($"Result: {result}");
        }
    }
}
