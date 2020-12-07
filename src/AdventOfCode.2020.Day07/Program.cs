using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("input.txt");

var bagColorRegex = new Regex(@"^(\w+ \w+)");
var propertiesRegex = new Regex(@"(\d+) (\w+ \w+)");

Dictionary<string, (int Count, string Color)[]> allBags = new();

foreach (var line in input)
{
    var color = bagColorRegex.Match(line).Groups[1].Value;
    var props = propertiesRegex.Matches(line).Select(m => (int.Parse(m.Groups[1].Value), m.Groups[2].Value)).ToArray();
    allBags.Add(color, props);
}

string[] GetBagsThatCanContainOne(string color)
{
    var bagsDirect = allBags.Where(b => b.Value.Any(r => r.Color == color && r.Count >= 1));
    var bagsIndirect = bagsDirect.SelectMany(b => GetBagsThatCanContainOne(b.Key));

    return bagsDirect.Select(b => b.Key).Concat(bagsIndirect).Distinct().ToArray();
}

Console.WriteLine($"Part 1: {GetBagsThatCanContainOne("shiny gold").Length}");
    