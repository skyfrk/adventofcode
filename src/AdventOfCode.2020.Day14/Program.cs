using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("input.txt");

var maskRegex = new Regex(@"mask = ([01X]+)");
var memRegex = new Regex(@"mem\[(\d+)\] = (\d+)");

void SolvePart1(){
    string currentMask = null;
    Dictionary<string, long> memory = new();

    foreach (var line in input)
    {
        if (maskRegex.IsMatch(line)) currentMask = maskRegex.Match(line).Groups[1].Value;

        if (memRegex.IsMatch(line))
        {
            var captureGroups = memRegex.Match(line).Groups;

            var address = captureGroups[1].Value;
            var value = uint.Parse(captureGroups[2].Value);

            var valueBitArray = Convert.ToString(value, 2).PadLeft(36, '0').ToArray();

            for (int i = 0; i < currentMask.Length; i++)
            {
                valueBitArray[i] = currentMask[i] == 'X' ? valueBitArray[i] : currentMask[i];
            }

            var maskedValue = Convert.ToInt64(new string(valueBitArray), 2);

            memory[address] = maskedValue;
        }
    }

    Console.WriteLine($"Part 1: {memory.Sum(x => x.Value)}");
}

void SolvePart2()
{
    string currentMask = null;
    Dictionary<string, long> memory = new();

    foreach (var line in input)
    {
        if (maskRegex.IsMatch(line)) currentMask = maskRegex.Match(line).Groups[1].Value;

        if (memRegex.IsMatch(line))
        {
            var captureGroups = memRegex.Match(line).Groups;

            var address = uint.Parse(captureGroups[1].Value);
            var value = uint.Parse(captureGroups[2].Value);

            var addressBitArray = Convert.ToString(address, toBase: 2).PadLeft(36, '0').ToArray();

            // prepare current address
            for(int i = 0; i < currentMask.Length; i++)
            {
                if(currentMask[i] == '1') addressBitArray[i] = '1';
                if (currentMask[i] == 'X') addressBitArray[i] = 'X';
            }

            long possiblePermutationsCount = (long)BigInteger.Pow(2, currentMask.Count(c => c == 'X'));

            for (long i = 0; i < possiblePermutationsCount; i++)
            { 
                var currentNum = Convert.ToString(i, toBase: 2).PadLeft(addressBitArray.Count(c => c == 'X'), '0');

                var addressBitArrayCopy = new char[addressBitArray.Length];
                Array.Copy(addressBitArray, addressBitArrayCopy, addressBitArray.Length);

                var numIdx = 0;

                // replace floating bit markers
                for(int j = 0; j < addressBitArrayCopy.Length; j++)
                {
                    if (addressBitArrayCopy[j] == 'X')
                    {
                        addressBitArrayCopy[j] = currentNum[numIdx];
                        numIdx++;
                    }
                }

                memory[new string(addressBitArrayCopy)] = value;
            }
        }
    }

    Console.WriteLine($"Part 2: {memory.Sum(x => x.Value)}");
}

SolvePart1();
SolvePart2();

