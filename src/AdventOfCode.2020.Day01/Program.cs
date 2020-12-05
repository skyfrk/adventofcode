using System;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt").Select(str => int.Parse(str)).ToArray();

const int goal = 2020;

for(int i = 0; i < input.Length; i++)
{
    for(int j = i + 1; j < input.Length; j++)
    {
        if(input[i] + input[j] == goal) Console.WriteLine($"part 1: {input[i] * input[j]}");

        for (int k = j + 1; k < input.Length; k++)
        {
            if (input[i] + input[j] + input[k] == goal) Console.WriteLine($"part 2: {input[i] * input[j] * input[k]}");
        }
    }
}