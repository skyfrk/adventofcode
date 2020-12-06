using System;
using System.IO;
using System.Linq;

var input = File.ReadAllText("input.txt");

var part1 = input.Split("\r\n\r\n").Select(group => group.Replace("\r\n", string.Empty).Distinct().Count()).Sum();
var part2 = input.Split("\r\n\r\n").Select(group => group.Split("\r\n").Aggregate((acc, x) => string.Concat(acc.Intersect(x))).Count()).Sum();

Console.WriteLine($"Part 1: {part1}\nPart 2: {part2}");