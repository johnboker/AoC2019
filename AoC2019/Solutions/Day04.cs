using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2019.Solutions
{
    public class Day04 : ISolution
    {
        public void Initialize(string filename)
        {
            
        }

        public void Solve1()
        {
            var cnt = 0;
            for (var i = 246515; i <= 739105; i++)
            {
                cnt += MeetsCriteria1(i) ? 1 : 0;
            }

            Console.WriteLine(cnt);
        }

        public void Solve2()
        {
            var cnt = 0;
            for (var i = 246515; i <= 739105; i++)
            {
                cnt += MeetsCriteria2(i) ? 1 : 0;
            }

            Console.WriteLine(cnt);
        }


        private bool MeetsCriteria1(int password)
        {
            var pw = password.ToString();
            if (pw.Length != 6) return false;

            var pass = false;
            for (int i = 0; i < pw.Length - 1; i++)
            {
                if (pw[i] == pw[i + 1])
                {
                    pass = true;
                    break;
                }
            }
            if (!pass) return false;

            for (int i = 0; i < pw.Length - 1; i++)
            {
                if (pw[i] != pw[i + 1] && pw[i] > pw[i + 1])
                {
                    return false;
                }
            }

            return true;
        }

        private bool MeetsCriteria2(int password)
        {
            var pw = password.ToString();
            if (pw.Length != 6) return false;

            var pass = false;
            var repeatingChars = new List<char>();
            for (int i = 0; i < pw.Length - 1; i++)
            {
                if (pw[i] == pw[i + 1])
                {
                    pass = true;
                    repeatingChars.Add(pw[i]);
                }
            }
            if (!pass) return false;

            for (int i = 0; i < pw.Length - 1; i++)
            {
                if (pw[i] != pw[i + 1] && pw[i] > pw[i + 1])
                {
                    return false;
                }
            }

            foreach (var c in repeatingChars)
            {
                if (pw.Where(a => a == c).Count() == 2) return true;
            }

            return false;
        }
    }
}
