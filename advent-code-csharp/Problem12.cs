

namespace Advent.Code
{

    public class Path
    {
        public List<string> names;

        public Path()
        {
            names = new List<string>();
        }
        
        public bool HasPaths()
        {
            return names.Count > 0;
        }

        public bool LastPathIsCave(Cave c)
        {
            return names[names.Count - 1] == c.Name;
        }

        public void Add(string name)
        {
            names.Add(name);
        }

        public void DeletePath()
        {
            names.Clear();
        }

        public void DeleteLast()
        {
            names.RemoveAt(names.Count - 1);
        }

        override public string ToString()
        {
            string s = "";
            bool notFirst = false;
            foreach (var name in names)
            {
                if (notFirst)
                {
                    s += "-";
                }

                s += name;
                notFirst = true;
            }
            return s;
        }
    }

    public class Cave
    {
        public string Name;
        public bool Big;
        public List<Cave> Connections;
        public Cave(string name, bool big)
        {
            Name = name;
            Big = big;
            Connections = new List<Cave>();
        }
    }

    public class Problem12
    {
        static readonly string EXAMPLE_FILE = "../../../exampleProblem12.txt";
        static readonly string INPUT_FILE = "../../../inputProblem12.txt";
        static readonly uint MAX_SMALL_CAVES_VISITS = 2;
        static Dictionary<string, Cave> caves = new();

        static List<Path> completedPaths = new(); // hashSet slower and fails when there are like >150k paths
        static Dictionary<string, uint> tinyCavesVisited = new();
        static readonly Path currentPath = new();
        public static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch watch = new();
            watch.Start();
            foreach (var s in File.ReadAllLines(INPUT_FILE))
            {
                CreateCaves(s, out Cave cave1, out Cave cave2);
            }

            IterateRecursively(caves["start"]);

            Console.WriteLine("Total paths {0}\n", completedPaths.Count());
            watch.Stop();
            Console.WriteLine($"Execution time {watch.ElapsedMilliseconds}ms");
        }

        private static void IterateRecursively(Cave c)
        {
            if (c.Connections.Count == 0)
            {
                return;
            }

            if (c.Name == "end")
            {
                currentPath.Add(c.Name);
                completedPaths.Add(currentPath);

#if DEBUG
                Console.WriteLine("Trying to add finished path {0}", currentPath.ToString());
#endif

                currentPath.DeleteLast();
                return;
            }

            if (!c.Big) tinyCavesVisited[c.Name] += 1;

            currentPath.Add(c.Name);

            foreach (var child in c.Connections)
            {
                if (child.Name == "start") continue;

                if (CanCaveBeVisitedAgain(child))
                {
                    IterateRecursively(child);
                    if(tinyCavesVisited[child.Name] > 0) tinyCavesVisited[child.Name]--;
                    if(currentPath.HasPaths() && !currentPath.LastPathIsCave(c)) currentPath.DeleteLast();
                }
            }
        }

        private static bool CanCaveBeVisitedAgain(Cave c)
        {
            uint currentMaxSmallCavesVisits = MAX_SMALL_CAVES_VISITS;
            if (tinyCavesVisited.ContainsValue(2)) currentMaxSmallCavesVisits = MAX_SMALL_CAVES_VISITS - 1;
            return tinyCavesVisited[c.Name] < currentMaxSmallCavesVisits;
        }

        private static void CreateCaves(in string line, out Cave cave1, out Cave cave2)
        {
            string[] linesplit = line.Split('-');
            string name1 = linesplit[0];
            string name2 = linesplit[1];

            if(!caves.ContainsKey(name1))
            {
                cave1 = new Cave(name1, IsBigCave(name1));
                tinyCavesVisited.Add(name1, 0);
                caves[name1] = cave1;
            } else
            {
                cave1 = caves[name1];
            }
            if(!caves.ContainsKey(name2))
            {
                cave2 = new Cave(name2, IsBigCave(name2));
                tinyCavesVisited.Add(name2, 0);
                caves[name2] = cave2;
            } else
            {
                cave2 = caves[name2];
            }

            cave1.Connections.Add(cave2);
            cave2.Connections.Add(cave1);
        }

        private static bool IsBigCave(string name)
        {
            return name.ToUpper() == name;
        }
    }

}