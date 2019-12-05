using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
            else if (!args.Contains("-all"))
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
                        var actions = new Action[] { solution.Solve1, solution.Solve2 };
                        for (int i = 0; i < actions.Count(); i++)
                        {
                            Console.Write($"Part {i + 1}:\t");
                            var executionTime = Execute(actions[i]);
                            Console.WriteLine($"[{executionTime} ms]\n");
                        }
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

        private static long Execute(Action action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();
            action.Invoke();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        private static ISolution GetSolution(int day)
        {
            var className = $"{Assembly.GetExecutingAssembly().GetName().Name}.Solutions.Day{day:00}";
            var type = Type.GetType(className);
            var solution = type == null ? null : Activator.CreateInstance(type) as ISolution;
            return solution;
        }
    }
}
