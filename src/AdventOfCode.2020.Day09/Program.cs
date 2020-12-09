using System;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt").Select(line => long.Parse(line)).ToArray();

for (int i = 25; i < input.Length; i++)
{
    var prevNums = input[(i - 25)..i];
    if (prevNums.SelectMany(num => prevNums.Where(x => x != num).Select(num2 => num2 + num)).Contains(input[i])) continue;
    Console.WriteLine($"Part 1: {input[i]}");
    break;
}