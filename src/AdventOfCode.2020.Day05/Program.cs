using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt");

var seats = new List<(int row, int column)>();

foreach (var line in input)
{
    var upperRow = 127;
    var lowerRow = 0;

    for (int i = 0; i < 7; i++)
    {
        double diff = upperRow - lowerRow;
        if (line[i] == 'F') upperRow -= (int)Math.Ceiling(diff / 2);
        if (line[i] == 'B') lowerRow += (int)Math.Ceiling(diff / 2);
    }

    var upperColumn = 7;
    var lowerColumn = 0;

    for (int i = 7; i < 10; i++)
    {
        double diff = upperColumn - lowerColumn;
        if (line[i] == 'L') upperColumn -= (int)Math.Ceiling(diff / 2);
        if (line[i] == 'R') lowerColumn += (int)Math.Ceiling(diff / 2);
    }

    var seatId = upperRow * 8 + upperColumn;

    seats.Add((upperRow, upperColumn));
}

var maxSeatId = seats.Select(seat => seat.row * 8 + seat.column).Max();

Console.WriteLine($"Part 1: {maxSeatId}");

var takenSeats = seats
    .Where(seat => seat.row != 0 && seat.row != 127)
    .Select(seat => seat.row * 8 + seat.column);

var mySeatId = Enumerable
    .Range(takenSeats.Min(), takenSeats.Count() + 1)
    .Except(takenSeats)
    .First();

Console.WriteLine($"Part 2: {mySeatId}");