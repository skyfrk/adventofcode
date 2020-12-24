using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt");

List<Tile> flatTilesList = new();

var referenceTile = new Tile();

flatTilesList.Add(referenceTile);

foreach (var line in input)
{
    var currentTile = referenceTile;

    char prev = '_';

    foreach (var letter in line)
    {
        if (letter is 's' or 'n')
        {
            prev = letter;
            continue;
        }

        if (letter is 'e')
        {
            if(prev is 's')
            {
                if(currentTile.SoutheastNeighbor is null)
                {
                    currentTile.SoutheastNeighbor = new Tile();
                    currentTile = currentTile.SoutheastNeighbor;
                }
                else
                {
                    currentTile.WhiteSideIsUp = !currentTile.WhiteSideIsUp;
                    currentTile = currentTile.SoutheastNeighbor;
                }
            } 
            else if (prev is 'n')
            {
                if (currentTile.NortheastNeighbor is null)
                {
                    currentTile.NortheastNeighbor = new Tile();
                    currentTile = currentTile.NortheastNeighbor;
                }
                else
                {
                    currentTile.WhiteSideIsUp = !currentTile.WhiteSideIsUp;
                    currentTile = currentTile.NortheastNeighbor;
                }
            }
            else
            {
                if (currentTile.EastNeighbor is null)
                {
                    currentTile.EastNeighbor = new Tile();
                    currentTile = currentTile.EastNeighbor;
                }
                else
                {
                    currentTile.WhiteSideIsUp = !currentTile.WhiteSideIsUp;
                    currentTile = currentTile.EastNeighbor;
                }
            }
        }

        if (letter is 'w')
        {
            if (prev is 's')
            {
                if (currentTile.SouthwestNeighbor is null)
                {
                    currentTile.SouthwestNeighbor = new Tile();
                    currentTile = currentTile.SouthwestNeighbor;
                }
                else
                {
                    currentTile.WhiteSideIsUp = !currentTile.WhiteSideIsUp;
                    currentTile = currentTile.SouthwestNeighbor;
                }
            }
            else if (prev is 'n')
            {
                if (currentTile.NorthwestNeighbor is null)
                {
                    currentTile.NorthwestNeighbor = new Tile();
                    currentTile = currentTile.NorthwestNeighbor;
                }
                else
                {
                    currentTile.WhiteSideIsUp = !currentTile.WhiteSideIsUp;
                    currentTile = currentTile.NorthwestNeighbor;
                }
            }
            else
            {
                if (currentTile.WestNeighbor is null)
                {
                    currentTile.WestNeighbor = new Tile();
                    currentTile = currentTile.WestNeighbor;
                }
                else
                {
                    currentTile.WhiteSideIsUp = !currentTile.WhiteSideIsUp;
                    currentTile = currentTile.WestNeighbor;
                }
            }
        }

        prev = '_';

        flatTilesList.Add(currentTile);
    }
}

Console.WriteLine($"Part 1: {flatTilesList.Count(t => !t.WhiteSideIsUp)}");

class Tile
{
    public bool WhiteSideIsUp { get; set; } = true;
    public Tile EastNeighbor { get; set; }
    public Tile SoutheastNeighbor { get; set; }
    public Tile SouthwestNeighbor { get; set; }
    public Tile WestNeighbor { get; set; }
    public Tile NorthwestNeighbor { get; set; }
    public Tile NortheastNeighbor { get; set; }
}