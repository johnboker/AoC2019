using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;


namespace AoC2019.Solutions
{
    public class Day09 : ISolution
    {
        private string InputFilename { get; set; }

        public void Initialize(string filename)
        {
            InputFilename = filename;
        }

        private List<BigInteger> GetInstructions()
        {
            var instructions = File.ReadAllText(InputFilename).Split(",")
                .Select(d => BigInteger.Parse(d))
                .ToList();

            instructions.AddRange(Enumerable.Repeat(0, 100000).Select(a => new BigInteger(a)));

            return instructions;
        }

        public void Solve1()
        {
            var computer = new IntcodeComputer
            {
                Instructions = GetInstructions(),
                Outputs = new Queue<BigInteger>(),
                Inputs = new Queue<BigInteger>()
            };

            computer.Inputs.Enqueue(1);

            computer.Run();

            while (computer.Outputs.Any())
            {
                Console.WriteLine(computer.Outputs.Dequeue());
            }
        }

        public void Solve2()
        {
            var computer = new IntcodeComputer
            {
                Instructions = GetInstructions(),
                Outputs = new Queue<BigInteger>(),
                Inputs = new Queue<BigInteger>()
            };

            computer.Inputs.Enqueue(2);

            computer.Run();

            while (computer.Outputs.Any())
            {
                Console.WriteLine(computer.Outputs.Dequeue());
            }
        }

        public class IntcodeComputer
        {
            public List<BigInteger> Instructions { get; set; }

            public Queue<BigInteger> Outputs { get; set; }
            public Queue<BigInteger> Inputs { get; set; }

            public int RelativeBase { get; set; }

            private int PC { get; set; }

            public IntcodeComputer()
            {
                PC = 0;
                RelativeBase = 0;
            }

            public ExitCode Run()
            {
                try
                {
                    int opcode = 0;

                    while (opcode != 99)
                    {
                        opcode = (int)(Instructions[PC]) % 100;
                        int p1mode = (int)((Instructions[PC] / 100) % 10);
                        int p2mode = (int)((Instructions[PC] / 1000) % 10);
                        int p3mode = (int)((Instructions[PC] / 10000) % 10);

                        BigInteger GetValue(int m, BigInteger v, int mode = 0)
                        {
                            BigInteger a = 0;
                            if (mode == 0)
                            {
                                switch (m)
                                {
                                    case 0:
                                        a = Instructions[(int)v];
                                        break;
                                    case 1:
                                        a = v;
                                        break;
                                    case 2:
                                        a = Instructions[(int)v + RelativeBase];
                                        break;
                                }
                            }
                            else
                            {
                                switch (m)
                                {
                                    case 0:
                                        a = v;
                                        break;
                                    case 1:
                                        a = v;
                                        break;
                                    case 2:
                                        a = (int)v + RelativeBase;
                                        break;
                                }
                            }
                            return a;
                        }

                        switch (opcode)
                        {
                            case 1:
                                Instructions[(int)GetValue(p3mode, Instructions[PC + 3], 1)] = GetValue(p1mode, Instructions[PC + 1]) + GetValue(p2mode, Instructions[PC + 2]);
                                PC += 4;
                                break;
                            case 2:
                                Instructions[(int)GetValue(p3mode, Instructions[PC + 3], 1)] = GetValue(p1mode, Instructions[PC + 1]) * GetValue(p2mode, Instructions[PC + 2]);
                                PC += 4;
                                break;
                            case 3:
                                if (Inputs.Any())
                                {
                                    Instructions[(int)GetValue(p1mode, Instructions[PC + 1], 1)] = Inputs.Dequeue();
                                    PC += 2;
                                }
                                else
                                {
                                    return ExitCode.NEED_INPUT;
                                }
                                break;
                            case 4:
                                Outputs.Enqueue(GetValue(p1mode, Instructions[PC + 1]));
                                PC += 2;
                                break;
                            case 5:
                                PC = (int)(GetValue(p1mode, Instructions[PC + 1]) != 0 ? GetValue(p2mode, Instructions[PC + 2]) : (PC + 3));
                                break;
                            case 6:
                                PC = (int)(GetValue(p1mode, Instructions[PC + 1]) == 0 ? GetValue(p2mode, Instructions[PC + 2]) : (PC + 3));
                                break;
                            case 7:
                                Instructions[(int)GetValue(p3mode, Instructions[PC + 3], 1)] = GetValue(p1mode, Instructions[PC + 1]) < GetValue(p2mode, Instructions[PC + 2]) ? 1 : 0;
                                PC += 4;
                                break;
                            case 8:
                                Instructions[(int)GetValue(p3mode, Instructions[PC + 3], 1)] = GetValue(p1mode, Instructions[PC + 1]) == GetValue(p2mode, Instructions[PC + 2]) ? 1 : 0;
                                PC += 4;
                                break;
                            case 9:
                                RelativeBase += (int)GetValue(p1mode, Instructions[PC + 1]);
                                PC += 2;
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
    }
}
