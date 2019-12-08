using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2019.Solutions
{
    public class Day08 : ISolution
    {
        private string Input { get; set; }
        public void Initialize(string filename)
        {
            Input = File.ReadAllText(filename);
        }

        public void Solve1()
        {
            var width = 25;
            var height = 6;

            var layerCount = Input.Count() / (width * height);
            var layers = new List<List<string>>();
            for (int i = 0; i < layerCount; i++)
            {
                //Console.WriteLine($"Layer {i + 1}");
                var layer = new List<string>();
                for (int r = 0; r < height; r++)
                {
                    var row = Input.Substring(i * width * height + r * width, width);
                    layer.Add(row);
                }
                //PrintLayer(layer);
                layers.Add(layer);
            }

            var data = layers.Select(a => string.Join("", a)).OrderBy(a => a.Count(b => b == '0')).FirstOrDefault();

            Console.WriteLine(data.Count(a => a == '1') * data.Count(a => a == '2'));
        }

        public void Solve2()
        {
            var width = 25;
            var height = 6;


            var layerCount = Input.Count() / (width * height);
            var layers = new List<List<string>>();
            for (int i = 0; i < layerCount; i++)
            {
                //Console.WriteLine($"Layer {i + 1}");
                var layer = new List<string>();
                for (int r = 0; r < height; r++)
                {
                    var row = Input.Substring(i * width * height + r * width, width);
                    layer.Add(row);
                }
                //PrintLayer(layer);
                layers.Add(layer);
            }

            var finalLayer = new List<List<char>>();
            for (int c = 0; c < height; c++)
            {
                finalLayer.Add(Enumerable.Repeat('2', width).ToList());
            }

            for (int i = 0; i < layers.Count(); i++)
            {
                var layer = layers[i];
                for (var c = 0; c < width; c++)
                {
                    for (var r = 0; r < height; r++)
                    {
                        var pixel = finalLayer[r][c];
                        if (pixel == '2')
                        {
                            finalLayer[r][c] = layer[r][c];
                        }
                    }
                }
            }
            Console.WriteLine("Final");
            PrintLayer(finalLayer.Select(a => new string(a.ToArray())).ToList());
        }

        public void PrintLayer(List<string> layer)
        {
            foreach (var row in layer)
            {
                foreach(var c in row)
                {
                    Console.Write(c != '0' ? '▓' : ' ');
                }
                Console.WriteLine();
            }
        }
    }
}
