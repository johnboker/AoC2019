using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2019.Solutions
{
    public class Day03 : ISolution
    {
        private List<List<(char Direction, int Distance)>> Wires { get; set; }

        public void Initialize(string filename)
        {
            Wires = File.ReadAllLines(filename)
                    .Select(a => a.Split(",")
                                  .Select(d => (Convert.ToChar(d.Substring(0, 1)), Convert.ToInt32(d.Substring(1))))
                                  .ToList())
                    .ToList();
        }

        public void Solve1()
        {
            var visited1 = GetVisited(Wires[0]);
            var visited2 = GetVisited(Wires[1]);

            var intersections = (from v1 in visited1
                                 join v2 in visited2 on new { v1.X, v1.Y } equals new { v2.X, v2.Y }
                                 select v1);


            var shortest = intersections.Select(a => Math.Abs(a.X) + Math.Abs(a.Y)).Where(a => a != 0).Min();

            Console.WriteLine(shortest);

        }



        public void Solve2()
        {
            var visited1 = GetVisited(Wires[0]);
            var visited2 = GetVisited(Wires[1]);

            var intersections = (from v1 in visited1
                                 join v2 in visited2 on new { v1.X, v1.Y } equals new { v2.X, v2.Y }
                                 select (X: v1.X, Y: v1.Y, Steps: v1.Steps + v2.Steps)).OrderBy(a => a.Steps);


            var shortest = intersections.Where(a => (Math.Abs(a.X) + Math.Abs(a.Y)) != 0).FirstOrDefault();

            Console.WriteLine(shortest.Steps);
        }


        private List<(int X, int Y, int Steps)> GetVisited(List<(char Direction, int Distance)> wire)
        {
            var current = (X: 0, Y: 0, Steps: 0);
            var visited = new List<(int X, int Y, int Steps)> { current };
            var steps = 0;

            foreach (var m in wire)
            {
                var distance = m.Distance;
                var direction = m.Direction == 'U' || m.Direction == 'R' ? 1 : -1;

                while (distance > 0)
                {
                    steps++;
                    if (m.Direction == 'U' || m.Direction == 'D')
                    {
                        current.Y += direction;
                    }
                    else
                    {
                        current.X += direction;
                    }

                    distance--;
                    visited.Add((current.X, current.Y, steps));
                }
            }

            return visited;
        }
    }
}
