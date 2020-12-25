using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt");

var parsedInput = new Direction[input.Length][];

for(int i = 0; i < input.Length; i++)
{
    var lineList = new List<Direction>();

    char prev = '_';

    foreach (var letter in input[i])
    {
        if (letter is 's' or 'n')
        {
            prev = letter;
            continue;
        }

        if (letter is 'e')
        {
            if(prev is 's') lineList.Add(Direction.Southeast);
            else if (prev is 'n') lineList.Add(Direction.Northeast);
            else lineList.Add(Direction.East);
        }

        if (letter is 'w')
        {
            if (prev is 's') lineList.Add(Direction.Southwest);
            else if (prev is 'n') lineList.Add(Direction.Northwest);
            else lineList.Add(Direction.West);
        }

        prev = '_';
    }

    parsedInput[i] = lineList.ToArray();
}

// position => isWhite map
Dictionary<(int x, int y), bool> tiles = new();

tiles.Add((0, 0), true);

foreach (var instruction in parsedInput)
{
    (int x, int y) position = (0, 0);

    foreach (var direction in instruction)
    {
        position = GetPosition(position, direction);
    }

    if (tiles.TryGetValue(position, out var isWhite))
    {
        tiles[position] = !isWhite;
    }
    else
    {
        tiles.Add(position, false);
    }
}

Console.WriteLine($"Part 1: {tiles.Count(t => !t.Value)}");

static (int x, int y) GetPosition((int x, int y) from, Direction toDirection)
{
    var (x, y) = from;

    var hexGridOffset = 0;

    if (y % 2 == 0) hexGridOffset = 1;

    return toDirection switch
    {
        Direction.East => (x - 1, y),
        Direction.West => (x + 1, y),
        Direction.Northeast => (x - hexGridOffset, y - 1),
        Direction.Northwest => (x + 1 - hexGridOffset, y - 1),
        Direction.Southeast => (x - hexGridOffset, y + 1),
        Direction.Southwest => (x + 1 - hexGridOffset, y + 1),
        _ => throw new InvalidOperationException()
    };
}

enum Direction
{
    East,
    Southeast,
    Southwest,
    West,
    Northwest,
    Northeast
}