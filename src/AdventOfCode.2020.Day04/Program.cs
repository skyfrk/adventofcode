using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("input.txt");


List<Dictionary<string, string>> passports = new();
Dictionary<string, string> currentPassport = new();

foreach (var line in input)
{
    if (line.Trim() == string.Empty)
    {
        passports.Add(currentPassport);
        currentPassport = new();
    } 
    else
    {
        var keyValuePairs = line.Split(' ');
        foreach (var pair in keyValuePairs)
        {
            var pairSplitted = pair.Split(':');
            currentPassport.Add(pairSplitted[0], pairSplitted[1]);
        }
    }
}

passports.Add(currentPassport);

void Part1()
{
    var validCount = 0;

    foreach (var p in passports)
    {
        if (
            p.ContainsKey("byr")
            && p.ContainsKey("iyr")
            && p.ContainsKey("eyr")
            && p.ContainsKey("hgt")
            && p.ContainsKey("hcl")
            && p.ContainsKey("ecl")
            && p.ContainsKey("pid"))
        {
            validCount++;
        }
    }

    Console.WriteLine($"Part 1: {validCount}");
}

void Part2()
{
    var validCount = 0;

    foreach (var p in passports)
    {
        if (
            !p.ContainsKey("byr")
            || !p.ContainsKey("iyr")
            || !p.ContainsKey("eyr")
            || !p.ContainsKey("hgt")
            || !p.ContainsKey("hcl")
            || !p.ContainsKey("ecl")
            || !p.ContainsKey("pid"))
        {
            continue;
        }

        var birthYear = int.Parse(p["byr"]);
        var issueYear = int.Parse(p["iyr"]);
        var expirationYear = int.Parse(p["eyr"]);
        var height = p["hgt"];
        var hairColor = p["hcl"];
        var eyeColor = p["ecl"];
        var passportId = p["pid"];

        if (birthYear < 1920 || birthYear > 2002) continue;
        if (issueYear < 2010 || issueYear > 2020) continue;
        if (expirationYear < 2020 || expirationYear > 2030) continue;

        var heightRegex = new Regex(@"([0-9]+)(cm|in)");
        var heightRegexResult = heightRegex.Match(height);
        if (!heightRegexResult.Success) continue;
        var heightValue = int.Parse(heightRegexResult.Groups[1].Value);
        var heightUnit = heightRegexResult.Groups[2].Value;
        if (heightUnit == "cm" && (heightValue < 150 || heightValue > 193)) continue;
        if (heightUnit == "in" && (heightValue < 59 || heightValue > 76)) continue;

        var hexColor = new Regex(@"^#([a-fA-F0-9]{6})$");
        if (!hexColor.IsMatch(hairColor)) continue;

        if (!new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(eyeColor)) continue;
        if (passportId.Trim().Length != 9) continue;

        validCount++;
    }

    Console.WriteLine($"Part 2: {validCount}");
}

Part1();
Part2();
