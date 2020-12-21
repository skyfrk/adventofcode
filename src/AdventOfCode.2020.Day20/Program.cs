using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var input = File.ReadAllText("sample.input.txt");

var tileIdRegex = new Regex(@"Tile (\d+):");

var tiles = input.Split("\r\n\r\n").Select(t =>
{
    var lines = t.Split("\r\n");
    return new Tile(int.Parse(tileIdRegex.Match(lines[0]).Groups[1].Value), lines[1..].Select(s => s.ToCharArray()).To2DArray());
}).ToList();


int tileEdgeLength = tiles[0].PictureEdgeLength;

var fullPictureEdgeLength = (int)Math.Sqrt(tiles.Count);

var fullPicture = new Tile[fullPictureEdgeLength, fullPictureEdgeLength];

fullPicture[0, 0] = tiles[0];
tiles.RemoveAt(0);

var iterationCount = 0;

//PrintTile(tiles[0]);
//tiles[0].Rotate();
//PrintTile(tiles[0]);
//tiles[0].Rotate();
//PrintTile(tiles[0]);
//tiles[0].Rotate();
//PrintTile(tiles[0]);

//PrintTile(tiles[0]);
//tiles[0].Flip();
//PrintTile(tiles[0]);


while (TryGetNextEmptySpot(out var position))
{
    PrintFullPicture(iterationCount);
    PrintFullPictureTileIds();

    // first row doesn't care about the tiles above
    if (position.row == 0)
    {
        if (HasNeighborToTheLeft(position.row, position.column))
        {
            // try to find something that matches
            if (TryGetMatchForLeftTile(fullPicture[position.row, position.column - 1], out var idx))
            {
                fullPicture[position.row, position.column] = tiles[idx];
                tiles.RemoveAt(idx);
            }
            else
            {
                // if nothing matches move tiles in the row to the right
                ShiftTilesInARowToTheRight(position.row);
            }
        }
        else
        {
            var emptyColIdx = GetColIndexWithNeighborToTheRight(position.row, position.column);

            if (TryGetMatchForRightTile(fullPicture[position.row, emptyColIdx + 1], out var idx))
            {
                fullPicture[position.row, emptyColIdx] = tiles[idx];
                tiles.RemoveAt(idx);
            }
            else
            {
                throw new InvalidOperationException("fuck");
            }
        }
    }
    else
    {
        Console.WriteLine("first row complete");
        // first row should be complete now
    }

    iterationCount++;
}


void PrintTile(Tile tile)
{
    Console.WriteLine($"Tile id: {tile.Id}");

    for(int row = 0; row < tileEdgeLength; row++)
    {
        var printStr = string.Empty;

        for (int col = 0; col < tileEdgeLength; col++)
        {
            printStr += tile.Picture[row, col];
        }
        
        Console.WriteLine(printStr);
    }
    Console.WriteLine(string.Empty);
}

void PrintFullPictureTileIds()
{
    Console.WriteLine(string.Empty);

    for(int row = 0; row < fullPictureEdgeLength; row++)
    {
        var colStr = string.Empty;

        for (int col = 0; col < fullPictureEdgeLength; col++)
        {
            var tile = fullPicture[row, col];

            if(tile == null)
            {
                colStr += " null";
            }
            else
            {
                colStr += $" {tile.Id}";
            }
        }

        Console.WriteLine(colStr);
        Console.WriteLine(string.Empty);
    }
}

void PrintFullPicture(int iterationCount)
{
    Console.Clear();
    Console.WriteLine($"Iteration: {iterationCount}");
    Console.WriteLine(string.Empty);

    for(int fullPicRow = 0; fullPicRow < fullPictureEdgeLength; fullPicRow++)
    {
        for (int tileRow = 0; tileRow < tileEdgeLength; tileRow++)
        {
            var tileRowFullPictureString = string.Empty;

            for (int fullPictureCol = 0; fullPictureCol < fullPictureEdgeLength; fullPictureCol++)
            {
                var tile = fullPicture[fullPicRow, fullPictureCol];

                if(tile == null)
                {
                    tileRowFullPictureString += new string(Enumerable.Range(0, tileEdgeLength).Select(x => '0').ToArray());
                }
                else
                {
                    for(int tileCol = 0; tileCol < tileEdgeLength; tileCol++)
                    {
                        tileRowFullPictureString += tile.Picture[tileRow, tileCol];
                    }
                }

                tileRowFullPictureString += " ";
            }

            Console.WriteLine(tileRowFullPictureString);
        }

        Console.WriteLine(string.Empty);
    }
}

int GetColIndexWithNeighborToTheRight(int row, int startCol)
{
    for(int col = startCol; col < fullPictureEdgeLength; col++)
    {
        if (col + 1 > fullPictureEdgeLength - 1) throw new InvalidOperationException("shit :(");
        if (fullPicture[row, col + 1] != null) return col;
    }
    throw new InvalidOperationException("shit :(");
}

bool HasNeighborToTheLeft(int row, int col)
{
    if (col - 1 < 0) return false;
    if (fullPicture[row, col - 1] == null) return false;
    return true;
}

void ShiftTilesInARowToTheRight(int row)
{
    var firstNullIndex = 0;

    for(int col = 0; col < fullPictureEdgeLength; col++)
    {
        if(fullPicture[row, col] == null)
        {
            firstNullIndex = col;
            break;
        }
    }

    var offset = fullPictureEdgeLength - firstNullIndex;

    for(int col = firstNullIndex - 1; col >= 0; col--)
    {
        fullPicture[row, col + offset] = fullPicture[row, col];
        fullPicture[row, col] = null;
    }
}

bool TryGetNextEmptySpot(out (int row, int column) position)
{
    for(int row = 0; row < fullPictureEdgeLength; row++)
    {
        for(int col = 0; col < fullPictureEdgeLength; col++)
        {
            if(fullPicture[row, col] == null)
            {
                position = (row, col);
                return true;
            }
        }
    }

    position = (0, 0);
    return false;
}


bool TryGetMatchForLeftTile(Tile left, out int matchingTileIdx)
{
    for(int i = 0; i < tiles.Count; i++)
    {
        for(int rotateCount = 0; rotateCount < 8; rotateCount++)
        {
            if(IsLeftRightMatch(left, tiles[i]))
            {
                matchingTileIdx = i;
                return true;
            }

            tiles[i].Rotate();

            if (rotateCount == 3) tiles[i].Flip();
        }
    }

    matchingTileIdx = -1;
    return false;
}

// this is probably broken
bool TryGetMatchForRightTile(Tile right, out int matchingTileIdx)
{
    for (int i = 0; i < tiles.Count; i++)
    {
        for (int rotateCount = 0; rotateCount < 4; rotateCount++)
        {
            if (IsLeftRightMatch(tiles[i], right))
            {
                matchingTileIdx = i;
                return true;
            }

            tiles[i].Rotate();

            if (rotateCount == 3) tiles[i].Flip();
        }
    }

    matchingTileIdx = -1;
    return false;
}

bool IsLeftRightMatch(Tile left, Tile right)
{
    for (int i = 0; i < tileEdgeLength; i++)
    {
        if (left.Picture[i, tileEdgeLength - 1] != right.Picture[i, 0]) return false;
    }
    return true;
}

bool IsAboveBelowMatch(Tile above, Tile below)
{
    for (int i = 0; i < tileEdgeLength; i++)
    {
        if (above.Picture[tileEdgeLength - 1, i] != below.Picture[0, i]) return false;
    }
    return true;
}

Console.WriteLine("Part 1: {}");
class Tile
{
    public int Id { get; set; }

    public char[,] Picture { get; set; }

    public int PictureEdgeLength { get; set; }

    public Tile(int id, char[,] picture)
    {
        Id = id;
        Picture = picture;
        PictureEdgeLength = (int)Math.Sqrt(picture.Length);
    }

    /// <summary>
    /// Rotate the tile 90 degrees to the right.
    /// </summary>
    public void Rotate()
    {
        char[,] rotatedPicture = new char[PictureEdgeLength, PictureEdgeLength];

        for (int i = 0; i < PictureEdgeLength; ++i)
        {
            for (int j = 0; j < PictureEdgeLength; ++j)
            {
                rotatedPicture[i, j] = Picture[PictureEdgeLength - j - 1, i];
            }
        }

        Picture = rotatedPicture;
    }

    public void Flip()
    {
        var length = (int)Math.Sqrt(Picture.Length);

        char[,] copy = new char[length, length];
        Array.Copy(Picture, copy, Picture.Length);

        for(int row = 0; row < length; row++)
        {
            for (int col = 0; col < length; col++)
            {
                Picture[row, col] = copy[length - 1 - row, col];
            }
        }

        Console.WriteLine("");
    }
}

static class EnumerableExtensions
{
    public static T[,] To2DArray<T>(this IEnumerable<IEnumerable<T>> source)
    {
        var data = source
            .Select(x => x.ToArray())
            .ToArray();

        var res = new T[data.Length, data.Max(x => x.Length)];
        for (var i = 0; i < data.Length; ++i)
        {
            for (var j = 0; j < data[i].Length; ++j)
            {
                res[i, j] = data[i][j];
            }
        }

        return res;
    }
}