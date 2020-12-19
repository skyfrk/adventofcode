using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("input.txt");

var messageRegex = new Regex(@"^([ab]+)$");
var ruleLineRegex = new Regex(@"^(\d+): (.*)");

List<string> messages = new();
Dictionary<int, string> rules = new();

foreach (var line in input)
{
    if (messageRegex.IsMatch(line)) messages.Add(line);
    if (ruleLineRegex.IsMatch(line))
    {
        var match = ruleLineRegex.Match(line);
        rules.Add(int.Parse(match.Groups[1].Value), match.Groups[2].Value);
    }
}

void SolvePart1()
{
    var firstRuleRegex = new Regex($"^{BuildRegex(rules[0])}$");

    Console.WriteLine($"Part 1: {messages.Count(m => firstRuleRegex.IsMatch(m))}");

    string BuildRegex(string inputRule)
    {
        if (inputRule.Contains("\"")) return inputRule.Replace("\"", string.Empty);

        List<string> subrules = new();

        if (inputRule.Contains("|"))
            subrules.AddRange(inputRule.Split("|").Select(s => s.Trim()));
        else
            subrules.Add(inputRule);

        List<string> subruleRegexList = new();

        foreach (var subrule in subrules)
        {
            var subruleRegex = string.Empty;
            foreach (var ruleIdStr in subrule.Split(" "))
            {
                var ruleId = int.Parse(ruleIdStr);
                subruleRegex += BuildRegex(rules[ruleId]);
            }
            subruleRegexList.Add(subruleRegex);
        }

        return $"({string.Join('|', subruleRegexList)})";
    }
}

void SolvePart2()
{

    var newRule8 = "newRule8Placeholder"; // 42 | 42 8 => rule 8 is now rule 8 or n-times rule 8
    var newRule11 = "newRule11Placeholder"; // 42 31 | 42 11 31 => 42 n-times + 31 n-times => i'll just use depth = 10 for testing

    rules[8] = newRule8;
    rules[11] = newRule11;
    var firstRuleRegex = new Regex($"^{BuildRegex(rules[0])}$");

    Console.WriteLine($"Part 2: {messages.Count(m => firstRuleRegex.IsMatch(m))}");

    string BuildRegex(string inputRule)
    {
        if(inputRule == newRule8)
        {
            return $"({BuildRegex("42")}+)";
        }

        if (inputRule == newRule11)
        {
            var regex42 = BuildRegex("42");
            var regex31 = BuildRegex("31");

            List<string> regexes = new();

            for(int i = 1; i <= 10; i++) // can also be more than 10. this is a very shitty solution - but it works!
            {
                regexes.Add($"({regex42}{{{i}}}{regex31}{{{i}}})");
            }
            
            return $"({string.Join('|', regexes)})";
        }

        if (inputRule.Contains("\"")) return inputRule.Replace("\"", string.Empty);

        List<string> subrules = new();

        if (inputRule.Contains("|"))
            subrules.AddRange(inputRule.Split("|").Select(s => s.Trim()));
        else
            subrules.Add(inputRule);

        List<string> subruleRegexList = new();

        foreach (var subrule in subrules)
        {
            var subruleRegex = string.Empty;
            foreach (var ruleIdStr in subrule.Split(" "))
            {
                var ruleId = int.Parse(ruleIdStr);
                subruleRegex += BuildRegex(rules[ruleId]);
            }
            subruleRegexList.Add(subruleRegex);
        }

        return $"({string.Join('|', subruleRegexList)})";
    }
}

SolvePart1();
SolvePart2();