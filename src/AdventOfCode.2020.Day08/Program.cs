using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("input.txt");

var instructionRegex = new Regex(@"^(nop|acc|jmp){1} ([\+|-]{1}\d+)$");

void SolvePart1()
{
    var instructions = input
    .Select(line =>
    {
        var match = instructionRegex.Match(line);
        var operation = match.Groups[1].Value;
        var value = int.Parse(match.Groups[2].Value);
        return (Operation: operation, Value: value, ExecutedCount: 0);
    })
    .ToArray();

    var accumulator = 0;

    for (int i = 0; i < instructions.Length; i++)
    {
        var (operation, value, executedCount) = instructions[i];

        if (executedCount > 0) break;

        instructions[i].ExecutedCount++;

        if (operation == "jmp")
        {
            i += value - 1;
        }

        if (operation == "acc")
        {
            accumulator += value;
        }
    }

    Console.WriteLine($"Part 1: {accumulator}");
}

void SolvePart2()
{
    var instructions = input
    .Select(line =>
    {
        var match = instructionRegex.Match(line);
        var operation = match.Groups[1].Value;
        var value = int.Parse(match.Groups[2].Value);
        return (Operation: operation, Value: value);
    }).ToArray();

    for (int i = 0; i < instructions.Length; i++)
    {
        var (operation, value) = instructions[i];

        if (operation == "acc") continue;

        var fixedOp = operation switch
        {
            "jmp" => "nop",
            "nop" => "jmp",
            _ => throw new InvalidOperationException()
        };

        var fixedInstructions = input
            .Select(line =>
            {
                var match = instructionRegex.Match(line);
                var operation = match.Groups[1].Value;
                var value = int.Parse(match.Groups[2].Value);
                return (Operation: operation, Value: value);
            }).ToArray();

        fixedInstructions[i].Operation = fixedOp;

        var result = Execute(fixedInstructions);

        if (result.Success)
        {
            Console.WriteLine($"Part 2: {result.Value}");
        }

    }

    static (int Value, bool Success) Execute((string operation, int value)[] input)
    {
        var instructions = input.Select(i => (Operation: i.operation, Value: i.value, Executed: false)).ToArray();

        var accumulator = 0;

        for (int i = 0; i < instructions.Length; i++)
        {
            var (operation, value, executed) = instructions[i];

            if (executed) return (0, false);

            instructions[i].Executed = true;

            if (operation == "jmp")
            {
                i += value - 1;
            }

            if (operation == "acc")
            {
                accumulator += value;
            }
        }

        return (accumulator, true);
    }
}

SolvePart1();
SolvePart2();