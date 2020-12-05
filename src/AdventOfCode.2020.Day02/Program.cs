using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("input.txt");

var regex = new Regex(@"(\d+)+-(\d+)+ (\w){1}: (\w+)");

var validPart1PasswordCount = 0;
var validPart2PasswordCount = 0;

foreach (var line in input)
{
    var groups = regex.Match(line).Groups;
    var min = int.Parse(groups[1].Value);
    var max = int.Parse(groups[2].Value);
    var letter = groups[3].Value[0];
    var password = groups[4].Value;
    var appearanceCount = password.Count(pwLetter => pwLetter == letter);

    if (appearanceCount >= min && appearanceCount <= max) validPart1PasswordCount++;

    if (max <= password.Length && min <= max && (password[min - 1] == letter ^ password[max - 1] == letter)) validPart2PasswordCount++;
}

Console.WriteLine($" {nameof(validPart1PasswordCount)} : {validPart1PasswordCount}");
Console.WriteLine($" {nameof(validPart2PasswordCount)} : {validPart2PasswordCount}");