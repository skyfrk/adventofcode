using System;
using System.Collections.Generic;
using System.IO;

var lines = File.ReadAllLines("input.txt");
var lineLength = lines[0].Length;

void SolvePart1()
{
    var count = 0;
    var idx = 0;

    for (int i = 1; i < lines.Length; i++)
    {
        idx += 3;
        if (idx >= lineLength) idx -= lineLength;
        if (lines[i][idx] == '#') count++;
    }

    Console.WriteLine($"Part 1: {count}");
}

void SolvePart2()
{
    var strats = new List<(int right, int down)>()
    {
        (1, 1),
        (3, 1),
        (5, 1),
        (7, 1),
        (1, 2)
    };

    long totalProduct = 1;

    foreach (var strat in strats)
    {
        var count = 0;
        var (right, down) = strat;
        var idx = 0;

        for (int i = down; i < lines.Length; i += down)
        {
            idx += right;
            if (idx >= lineLength) idx -= lineLength;
            if (lines[i][idx] == '#') count++;
        }

        totalProduct *= count;
    }

    Console.WriteLine($"Part 1: {totalProduct}");
}

SolvePart1();
SolvePart2();