using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2019.Solutions
{
    public class Day02 : ISolution
    {
        private string InputFilename { get; set; }
        private List<int> Input { get; set; }

        public void Initialize(string filename)
        {
            InputFilename = filename;
        }

        public void Solve1()
        {
            var result = Run(12, 2);

            if (result == Int32.MinValue) return;

            Console.WriteLine(result);
        }

        public void Solve2()
        {
            for (int noun = 0; noun <= 99; noun++)
            {
                for (int verb = 0; verb <= 99; verb++)
                {
                    var result = Run(noun, verb);

                    if (result == Int32.MinValue) return;

                    if (result == 19690720)
                    {
                        Console.WriteLine($"({noun}, {verb}) = {100 * noun + verb}");
                    }
                }
            }
        }

        private void InitializeInput()
        {
            Input = File.ReadAllText(InputFilename).Split(",")
                .Select(d => int.Parse(d))
                .ToList();
        }

        private int Run(int noun, int verb)
        {
            InitializeInput();

            int idx1, idx2, idx3;

            Input[1] = noun;
            Input[2] = verb;

            try
            {
                for (int i = 0; i < Input.Count(); i += 4)
                {
                    var opcode = Input[i];
                    switch (opcode)
                    {
                        case 1:
                            idx1 = Input[i + 1];
                            idx2 = Input[i + 2];
                            idx3 = Input[i + 3];
                            Input[idx3] = Input[idx1] + Input[idx2];
                            break;
                        case 2:
                            idx1 = Input[i + 1];
                            idx2 = Input[i + 2];
                            idx3 = Input[i + 3];
                            Input[idx3] = Input[idx1] * Input[idx2];
                            break;
                        case 99:
                            break;
                    }

                    if (opcode == 99) break;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid Program");
                return Int32.MinValue;
            }

            return Input[0];
        }
    }
}
