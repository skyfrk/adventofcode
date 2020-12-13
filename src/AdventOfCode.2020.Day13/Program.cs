using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("input.txt");

var earliestLeaveTimestamp = int.Parse(input[0]);

var busIdRegex = new Regex(@"(\d)+");

var busIds = busIdRegex.Matches(input[1]).Select(m => int.Parse(m.Value));

for(int i = earliestLeaveTimestamp; true; i++)
{
    foreach (var busId in busIds)
    {
        if(i % busId == 0)
        {
            var waitTime = i - earliestLeaveTimestamp;
            Console.WriteLine($"Part 1: {waitTime * busId}");
            Environment.Exit(0);
        }
    }
}