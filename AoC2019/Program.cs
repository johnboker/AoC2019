using System;
using System.IO;
using System.Linq;
using AoC2019.Solutions;

namespace AoC2019
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int day = DateTime.Now.Day;

            if (args.Length > 0)
            {
                day = Convert.ToInt32(args[0]);
            }

            var files = Directory.GetFiles($"input", $"day{day:00}*.txt");

            if (args.Contains("-test"))
            {
                files = files.Where(a => a.Contains("test")).ToArray();
            }
            else if(!args.Contains("-all"))
            {
                files = files.Where(a => !a.Contains("test")).ToArray();
            }

            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    var solution = GetSolution(day);
                    if (solution != null)
                    {
                        solution.Initialize(file);

                        Console.WriteLine($"\n****** {file} ******");

                        Console.Write("Part 1:\t");
                        solution.Solve1();

                        Console.Write("Part 2:\t");
                        solution.Solve2();

                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("No solution class found");
                        break;
                    }
                }
                else
                {
                    Console.Write($"{file} does not exist.");
                }
            }
        }

        private static ISolution GetSolution(int day)
        {
            var className = $"Solutions.Day{day:00}";
            var type = Type.GetType(className);
            var solution = type == null ? null : Activator.CreateInstance(type) as ISolution;
            return solution;
        }
    }
}
