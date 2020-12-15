using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt")[0].Split(',').Select(int.Parse).ToArray();

void SolvePart1()
{
    var targetNumber = 2020;
    var spokenNumbers = input.ToList();

    for (int i = input.Length; i < targetNumber; i++)
    {
        var prevNum = spokenNumbers[i - 1];

        var lastIdx = spokenNumbers.SkipLast(1).ToList().LastIndexOf(prevNum);

        if (lastIdx == -1)
        {
            spokenNumbers.Add(0);
        }
        else
        {
            spokenNumbers.Add(i - 1 - lastIdx);
        }
    }

    Console.WriteLine($"Part 1: {spokenNumbers[targetNumber - 1]}");
}

void SolvePart2()
{
    var targetNumber = 30000000;
    var memory = new Dictionary<int, int>();

    // seed memory
    for (int i = 0; i < input.Length - 1; i++) memory.Add(input[i], i);

    // start with last number from seed
    int number = input[input.Length - 1];

    for(int i = input.Length; i < targetNumber; i++)
    {
        if (!memory.TryGetValue(number, out var value))
        {
            memory[number] = i - 1;
            number = 0;
        }
        else
        {
            int newNumber = i - 1 - value;
            memory[number] = i - 1;
            number = newNumber;
        }
    }

    Console.WriteLine($"Part 2: {number}");
}

SolvePart1();
SolvePart2();
