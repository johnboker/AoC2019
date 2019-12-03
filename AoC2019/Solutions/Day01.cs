using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2019.Solutions
{
    public class Day01 : ISolution
    {
        private List<int> Input { get; set; }

        public void Initialize(string filename)
        {
            Input = File.ReadAllLines(filename)
                .Select(d => int.Parse(d))
                .ToList();
        }

        public void Solve1()
        {
            var fuel = Input.Sum(FuelForModule);
            Console.WriteLine(fuel);
        }

        public void Solve2()
        {
            var fuel = Input.Select(FuelForModuleAndFuel).Sum();
            Console.WriteLine(fuel);
        }

        private int FuelForModule(int m)
        {
            var f = m / 3 - 2;
            return f < 0 ? 0 : f;
        }

        private int FuelForModuleAndFuel(int m)
        {
            var f = FuelForModule(m);
            return f <= 0 ? 0 : (f + FuelForModuleAndFuel(f));
        }
    }
}
