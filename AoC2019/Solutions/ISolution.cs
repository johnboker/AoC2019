using System;
namespace AoC2019.Solutions
{
    public interface ISolution
    {
        void Initialize(string filename);
        void Solve1();
        void Solve2();
    }
}
