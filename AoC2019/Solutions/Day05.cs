using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2019.Solutions
{
    public class Day05 : ISolution
    {

        private List<int> Instructions { get; set; }
        private string InputFilename { get; set; }

        public void Initialize(string filename)
        {
            InputFilename = filename;
        }

        public void Solve1()
        {
            Run(1);
        }

        public void Solve2()
        {
            Run(5);
        }

        private void InitializeInstructions()
        {
            Instructions = File.ReadAllText(InputFilename).Split(",")
                .Select(d => int.Parse(d))
                .ToList();
        }

        private int Run(int input)
        {
            InitializeInstructions();

            try
            {
                int opcode = 0;

                int i = 0;
                while (opcode != 99)
                {
                    opcode = Instructions[i] % 100;
                    var p1mode = (Instructions[i] / 100) % 10;
                    var p2mode = (Instructions[i] / 1000) % 10;
                    var p3mode = (Instructions[i] / 10000) % 10;

                    int GetValue(int m, int v) => m != 0 ? v : Instructions[v];

                    switch (opcode)
                    {
                        case 1:
                            Instructions[Instructions[i + 3]] = GetValue(p1mode, Instructions[i + 1]) + GetValue(p2mode, Instructions[i + 2]);
                            i += 4;
                            break;
                        case 2:
                            Instructions[Instructions[i + 3]] = GetValue(p1mode, Instructions[i + 1]) * GetValue(p2mode, Instructions[i + 2]);
                            i += 4;
                            break;
                        case 3:
                            Instructions[Instructions[i + 1]] = input;
                            i += 2;
                            break;
                        case 4:
                            Console.WriteLine(GetValue(p1mode, Instructions[i + 1]));
                            i += 2;
                            break;
                        case 5:
                            i = GetValue(p1mode, Instructions[i + 1]) != 0 ? GetValue(p2mode, Instructions[i + 2]) : (i + 3);
                            break;
                        case 6:
                            i = GetValue(p1mode, Instructions[i + 1]) == 0 ? GetValue(p2mode, Instructions[i + 2]) : (i + 3);
                            break;
                        case 7:
                            Instructions[Instructions[i + 3]] = GetValue(p1mode, Instructions[i + 1]) < GetValue(p2mode, Instructions[i + 2]) ? 1 : 0;
                            i += 4;
                            break;
                        case 8:
                            Instructions[Instructions[i + 3]] = GetValue(p1mode, Instructions[i + 1]) == GetValue(p2mode, Instructions[i + 2]) ? 1 : 0;
                            i += 4;
                            break;
                        case 99:
                            break;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid Program");
                return Int32.MinValue;
            }

            return Instructions[0];
        }
    }
}
