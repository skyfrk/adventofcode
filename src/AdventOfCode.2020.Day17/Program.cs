using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt");

void SolvePart1()
{
    Dictionary<(int x, int y, int z), CubeState> currentState = new();

    // seed initial state from input
    for (int row = 0; row < input.Length; row++)
    {
        for (int column = 0; column < input[0].Length; column++)
        {

            var state = input[row][column] switch
            {
                '#' => CubeState.Active,
                '.' => CubeState.Inactive,
                _ => throw new InvalidOperationException("Invalid state.")
            };

            currentState.Add((column, row, 0), state);
        }
    }

    // add inactive cubes from t = 0
    var allInitialDirectNeighbors = currentState.SelectMany(x => GetDirectNeighbors(x.Key)).Distinct().ToDictionary(key => key.Key, value => value.Value);

    foreach (var neighbor in allInitialDirectNeighbors)
    {
        if (!currentState.ContainsKey(neighbor.Key))
        {
            currentState.Add(neighbor.Key, neighbor.Value);
        }
    }

    for (int iteration = 0; iteration < 6; iteration++)
    {
        Dictionary<(int x, int y, int z), CubeState> nextState = new();
        Dictionary<(int x, int y, int z), CubeState> allDirectNeighbors = new();

        foreach (var (position, cubeState) in currentState)
        {
            var directNeighbors = GetDirectNeighbors(position);
            var activeDirectNeighborCount = directNeighbors.Count(x => x.Value == CubeState.Active);

            foreach (var neighbor in directNeighbors)
            {
                if (!allDirectNeighbors.ContainsKey(neighbor.Key))
                {
                    allDirectNeighbors.Add(neighbor.Key, neighbor.Value);
                }
            }

            if (cubeState is CubeState.Active && activeDirectNeighborCount is < 2 or > 3)
            {
                nextState.Add(position, CubeState.Inactive);
            }
            else if (cubeState is CubeState.Inactive && activeDirectNeighborCount is 3)
            {
                nextState.Add(position, CubeState.Active);
            }
            else
            {
                nextState.Add(position, cubeState);
            }
        }

        foreach (var neighbor in allDirectNeighbors)
        {
            if (!nextState.ContainsKey(neighbor.Key))
            {
                nextState.Add(neighbor.Key, neighbor.Value);
            }
        }

        currentState = nextState;
    }

    Console.WriteLine($"Part 1: {currentState.Values.Count(state => state == CubeState.Active)}");

    Dictionary<(int x, int y, int z), CubeState> GetDirectNeighbors((int x, int y, int z) position)
    {
        var neighborOffsets = new List<(int x, int y, int z)>
    {
        (-1, -1, 1), (0, -1, 1), (1, -1, 1), (-1, 0, 1), (0, 0, 1), (1, 0, 1), (-1, 1, 1), (0, 1, 1), (1, 1, 1),
        (-1, -1, 0), (0, -1, 0), (1, -1, 0), (-1, 0, 0), (1, 0, 0), (-1, 1, 0), (0, 1, 0), (1, 1, 0), (-1, -1, -1),
        (0, -1, -1), (1, -1, -1), (-1, 0, -1), (0, 0, -1), (1, 0, -1), (-1, 1, -1), (0, 1, -1), (1, 1, -1)
    };

        return neighborOffsets.Select(offset =>
        {
            var newPosition = (x: position.x + offset.x, y: position.y + offset.y, z: position.z + offset.z);

            if (currentState.TryGetValue(newPosition, out var cubeState))
            {
                return (position: newPosition, state: cubeState);
            }
            else
            {
                return (position: newPosition, state: CubeState.Inactive);
            }
        }).ToDictionary(key => (x: key.position.x, y: key.position.y, z: key.position.z), value => value.state);

    }
}

void SolvePart2()
{
    Dictionary<(int x, int y, int z, int w), CubeState> currentState = new();

    // seed initial state from input
    for (int row = 0; row < input.Length; row++)
    {
        for (int column = 0; column < input[0].Length; column++)
        {

            var state = input[row][column] switch
            {
                '#' => CubeState.Active,
                '.' => CubeState.Inactive,
                _ => throw new InvalidOperationException("Invalid state.")
            };

            currentState.Add((column, row, 0, 0), state);
        }
    }

    // add inactive cubes from t = 0
    var allInitialDirectNeighbors = currentState.SelectMany(x => GetDirectNeighbors(x.Key)).Distinct().ToDictionary(key => key.Key, value => value.Value);

    foreach (var neighbor in allInitialDirectNeighbors)
    {
        if (!currentState.ContainsKey(neighbor.Key))
        {
            currentState.Add(neighbor.Key, neighbor.Value);
        }
    }

    for (int iteration = 0; iteration < 6; iteration++)
    {
        Dictionary<(int x, int y, int z, int w), CubeState> nextState = new();
        Dictionary<(int x, int y, int z, int w), CubeState> allDirectNeighbors = new();

        foreach (var (position, cubeState) in currentState)
        {
            var directNeighbors = GetDirectNeighbors(position);
            var activeDirectNeighborCount = directNeighbors.Count(x => x.Value == CubeState.Active);

            foreach (var neighbor in directNeighbors)
            {
                if (!allDirectNeighbors.ContainsKey(neighbor.Key))
                {
                    allDirectNeighbors.Add(neighbor.Key, neighbor.Value);
                }
            }

            if (cubeState is CubeState.Active && activeDirectNeighborCount is < 2 or > 3)
            {
                nextState.Add(position, CubeState.Inactive);
            }
            else if (cubeState is CubeState.Inactive && activeDirectNeighborCount is 3)
            {
                nextState.Add(position, CubeState.Active);
            }
            else
            {
                nextState.Add(position, cubeState);
            }
        }

        foreach (var neighbor in allDirectNeighbors)
        {
            if (!nextState.ContainsKey(neighbor.Key))
            {
                nextState.Add(neighbor.Key, neighbor.Value);
            }
        }

        currentState = nextState;
    }

    Console.WriteLine($"Part 2: {currentState.Values.Count(state => state == CubeState.Active)}");

    Dictionary<(int x, int y, int z, int w), CubeState> GetDirectNeighbors((int x, int y, int z, int w) position)
    {
        return GetNeighborOffsets().Select(offset =>
        {
            var newPosition = (x: position.x + offset.x, y: position.y + offset.y, z: position.z + offset.z, w: position.w + offset.w);

            if (currentState.TryGetValue(newPosition, out var cubeState))
            {
                return (position: newPosition, state: cubeState);
            }
            else
            {
                return (position: newPosition, state: CubeState.Inactive);
            }
        }).ToDictionary(key => (x: key.position.x, y: key.position.y, z: key.position.z, w: key.position.w), value => value.state);

    }

    IEnumerable<(int x, int y, int z, int w)> GetNeighborOffsets()
    {
        var result = new List<(int x, int y, int z, int w)>();
        var possibleValues = new[] { -1, 0, 1 };

        foreach (var x in possibleValues)
        {
            foreach (var y in possibleValues)
            {
                foreach (var z in possibleValues)
                {
                    foreach (var w in possibleValues)
                    {
                        result.Add((x, y, z, w));
                    }
                }
            }
        }

        return result.Where(x => x != (0, 0, 0, 0));
    }
}

SolvePart1();
SolvePart2();

enum CubeState
{
    Active,
    Inactive
}