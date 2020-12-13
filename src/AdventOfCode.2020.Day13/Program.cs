using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("input.txt");

void SolvePart1()
{
    var earliestLeaveTimestamp = int.Parse(input[0]);

    var busIdRegex = new Regex(@"(\d)+");

    var busIds = busIdRegex.Matches(input[1]).Select(m => int.Parse(m.Value));

    for (int i = earliestLeaveTimestamp; true; i++)
    {
        var foundSolution = false;

        foreach (var busId in busIds)
        {
            if (i % busId == 0)
            {
                var waitTime = i - earliestLeaveTimestamp;
                Console.WriteLine($"Part 1: {waitTime * busId}");
                foundSolution = true;
                break;
            }
        }
        if (foundSolution) break;
    }
}

void SolvePart2()
{
    var allBusIds = input[1].Split(',');

    var allAvailableBusIdsWithOffset = new List<(int id, int offset)>();

    for (int i = 0; i < allBusIds.Length; i++)
    {
        if (allBusIds[i] != "x")
        {
            allAvailableBusIdsWithOffset.Add((int.Parse(allBusIds[i]), i));
        }
    }

    long num = 1;
    long increment = 1;

    foreach (var (id, offset) in allAvailableBusIdsWithOffset)
    {
        while (true)
        {
            if ((num + offset) % id == 0)
            {
                increment *= id;
                break;
            }

            num += increment;
        }
    }

    Console.WriteLine($"Part 2: {num}");
}

SolvePart1();
SolvePart2();
