using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static AoC2019.Solutions.Day07.IntcodeComputer;

namespace AoC2019.Solutions
{
    public class Day07 : ISolution
    {
        private string InputFilename { get; set; }

        public void Initialize(string filename)
        {
            InputFilename = filename;
        }

        private List<int> GetInstructions()
        {
            return File.ReadAllText(InputFilename).Split(",")
                .Select(d => int.Parse(d))
                .ToList();
        }

        public void Solve1()
        {
            var allPhaseSettings = GetPermutations(new List<int> { 0, 1, 2, 3, 4 }, 5);
            var max = 0;
            foreach (var phases in allPhaseSettings)
            {
                var output = 0;
                foreach (var phase in phases)
                {
                    var amp = new Amplifier
                    {
                        IntcodeComputer = new IntcodeComputer
                        {
                            Instructions = GetInstructions(),
                            Outputs = new Queue<int>(),
                            Inputs = new Queue<int>()
                        }
                    };

                    amp.IntcodeComputer.Inputs.Enqueue(phase);
                    amp.IntcodeComputer.Inputs.Enqueue(output);

                    var exitCode = amp.IntcodeComputer.Run();

                    output = amp.IntcodeComputer.Outputs.Dequeue();
                }

                max = Math.Max(output, max);
            }
            Console.WriteLine($"Max: {max}");
        }

        public void Solve2()
        {
            var allPhaseSettings = GetPermutations(new List<int> { 5, 6, 7, 8, 9 }, 5);
            var max = 0;
            var output = 0;
            foreach (var phases in allPhaseSettings)
            {
                var amps = new List<Amplifier>();
                for (var i = 0; i < 5; i++)
                {
                    amps.Add(new Amplifier
                    {
                        Label = $"Amp {(char)('A' + i)}",
                        IntcodeComputer = new IntcodeComputer
                        {
                            Instructions = GetInstructions(),
                        }
                    });
                }

                for (var i = 0; i < 5; i++)
                {
                    amps[i].NextAmplifier = amps[(i + 1) % 5];
                    amps[i].PrevAmplifier = amps[((i - 1) % 5 + 5) % 5];
                    amps[i].IntcodeComputer.Outputs = new Queue<int>();
                    amps[i].NextAmplifier.IntcodeComputer.Inputs = amps[i].IntcodeComputer.Outputs;
                }


                for (var i = 0; i < 5; i++)
                {
                    amps[i].IntcodeComputer.Inputs.Enqueue(phases[i]);
                }

                amps[0].IntcodeComputer.Inputs.Enqueue(0);

                var exitCode = ExitCode.NEED_INPUT;

                while (exitCode == ExitCode.NEED_INPUT)
                {
                    for (var i = 0; i < 5; i++)
                    {
                        exitCode = amps[i].Run();
                        if (exitCode == ExitCode.HALT && i == 4)
                        {
                            break;
                        }
                    }
                }


                while (amps[4].IntcodeComputer.Outputs.Count() > 0)
                {
                    output = amps[4].IntcodeComputer.Outputs.Dequeue();
                }
                max = Math.Max(max, output);

            }
            Console.WriteLine($"Max: {max}");
        }




        public class Amplifier
        {
            public string Label { get; set; }
            public IntcodeComputer IntcodeComputer { get; set; }
            public Amplifier NextAmplifier { get; set; }
            public Amplifier PrevAmplifier { get; set; }

            public ExitCode Run()
            {
                return IntcodeComputer.Run();
            }
        }

        public class IntcodeComputer
        {
            public List<int> Instructions { get; set; }

            public Queue<int> Outputs { get; set; }
            public Queue<int> Inputs { get; set; }

            private int PC { get; set; }

            public ExitCode Run()
            {
                try
                {
                    int opcode = 0;

                    while (opcode != 99)
                    {
                        opcode = Instructions[PC] % 100;
                        var p1mode = (Instructions[PC] / 100) % 10;
                        var p2mode = (Instructions[PC] / 1000) % 10;
                        var p3mode = (Instructions[PC] / 10000) % 10;

                        int GetValue(int m, int v) => m != 0 ? v : Instructions[v];

                        switch (opcode)
                        {
                            case 1:
                                Instructions[Instructions[PC + 3]] = GetValue(p1mode, Instructions[PC + 1]) + GetValue(p2mode, Instructions[PC + 2]);
                                PC += 4;
                                break;
                            case 2:
                                Instructions[Instructions[PC + 3]] = GetValue(p1mode, Instructions[PC + 1]) * GetValue(p2mode, Instructions[PC + 2]);
                                PC += 4;
                                break;
                            case 3:
                                if (Inputs.Count() > 0)
                                {
                                    Instructions[Instructions[PC + 1]] = Inputs.Dequeue();
                                    PC += 2;
                                }
                                else
                                {
                                    return ExitCode.NEED_INPUT;
                                }
                                break;
                            case 4:
                                Outputs.Enqueue(GetValue(p1mode, Instructions[PC + 1]));
                                //Console.WriteLine($"Enqueued {Outputs.Peek()}");
                                PC += 2;
                                break;
                            case 5:
                                PC = GetValue(p1mode, Instructions[PC + 1]) != 0 ? GetValue(p2mode, Instructions[PC + 2]) : (PC + 3);
                                break;
                            case 6:
                                PC = GetValue(p1mode, Instructions[PC + 1]) == 0 ? GetValue(p2mode, Instructions[PC + 2]) : (PC + 3);
                                break;
                            case 7:
                                Instructions[Instructions[PC + 3]] = GetValue(p1mode, Instructions[PC + 1]) < GetValue(p2mode, Instructions[PC + 2]) ? 1 : 0;
                                PC += 4;
                                break;
                            case 8:
                                Instructions[Instructions[PC + 3]] = GetValue(p1mode, Instructions[PC + 1]) == GetValue(p2mode, Instructions[PC + 2]) ? 1 : 0;
                                PC += 4;
                                break;
                            case 99:
                                break;
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid Program");
                    return ExitCode.EXCEPTION;
                }

                return ExitCode.HALT;
            }

            public enum ExitCode
            {
                HALT,
                NEED_INPUT,
                EXCEPTION
            }
        }

        static List<List<T>> GetPermutations<T>(List<T> list, int length)
        {
            if (length == 1) return list.Select(t => new List<T> { t }).ToList();

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                (t1, t2) => t1.Concat(new List<T> { t2 }).ToList()).ToList();
        }
    }
}
