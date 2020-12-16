using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("input.txt");

var fieldRuleRegex = new Regex(@"([a-z ]+): (\d+)-(\d+) or (\d+)-(\d+)");
var ticketValueRegex = new Regex(@"^[0-9,]+$");

List<Field> fields = new();
List<int[]> tickets = new();

foreach (var line in input)
{
    if (line.Trim() == string.Empty) continue;

    if (fieldRuleRegex.IsMatch(line))
    {
        var matchGroups = fieldRuleRegex.Match(line).Groups;
        fields.Add(new Field(matchGroups[1].Value, new InclusiveRange[]
        {
            new InclusiveRange(int.Parse(matchGroups[2].Value), int.Parse(matchGroups[3].Value)),
            new InclusiveRange(int.Parse(matchGroups[4].Value), int.Parse(matchGroups[5].Value))
        }));
    }

    if (ticketValueRegex.IsMatch(line))
    {
        tickets.Add(line.Split(',').Select(int.Parse).ToArray());
    }
}

var invalidValuesSum = tickets
    .ToArray()[1..] // remove own ticket
    .SelectMany(t => t)
    // false added to often
    .Aggregate(0, (acc, value) => fields.Any(f => f.Validate(value)) ? acc : acc + value);

Console.WriteLine($"Part 1: {invalidValuesSum}");

record Field(string Name, InclusiveRange[] Ranges)
{
    public bool Validate(int value) => Ranges.Any(r => r.ContainsValue(value));
}

record InclusiveRange(int First, int Last)
{
    public bool ContainsValue(int value) => value >= First && value <= Last;
}
