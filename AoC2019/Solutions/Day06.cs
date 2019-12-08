using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2019.Solutions
{
    public class Day06 : ISolution
    {
        private List<OrbitInfo> MapData { get; set; }

        public void Initialize(string filename)
        {
            MapData = File.ReadAllLines(filename)
               .Select(a => new OrbitInfo()
               {
                   Name = a.Split(")")[0],
                   OrbitingMe = a.Split(")")[1]
               }).ToList();
        }

        public void Solve1()
        {
            var orbitCount = CountOrbits("COM", 0);
            Console.WriteLine(orbitCount);
        }

        private int CountOrbits(string name, int c)
        {
            var cnt = 0;
            var orbits = MapData.Where(a => a.Name == name);

            foreach (var o in orbits)
            {
                cnt += CountOrbits(o.OrbitingMe, c + 1);
            }

            return cnt + c;
        }

        public void Solve2()
        {
            var paths = new List<string>();
            FindPaths(new List<string> { "YOU" }, "YOU", "YOU", paths);
            var validPaths = paths.Where(a => a.StartsWith("YOU-", StringComparison.Ordinal) && a.EndsWith("-SAN", StringComparison.CurrentCulture));
            var shortest = validPaths.OrderBy(a => a.Length).FirstOrDefault();
            var length = shortest.Count(a => a == '-') - 2;
            Console.WriteLine(length);
        }


        private void FindPaths(List<string> visited, string from, string path, List<string> paths)
        {
            var orbits = MapData.Where(a => a.OrbitingMe == from || a.Name == from)
                                .SelectMany(a => new List<string> { a.Name, a.OrbitingMe });
            
            foreach (var o in orbits)
            {
                if (!visited.Contains(o))
                {
                    visited.Add(o);
                    FindPaths(visited, o, $"{path}-{o}", paths);
                    visited.Remove(o);
                }
            }

            paths.Add(path);
        }

        private class OrbitInfo
        {
            public string Name { get; set; }
            public string OrbitingMe { get; set; }
        }
    }
}
