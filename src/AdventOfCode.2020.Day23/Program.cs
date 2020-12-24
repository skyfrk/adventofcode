using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

void SolvePart1()
{
    var cups = File.ReadAllLines("input.txt")[0].Select(c => int.Parse(c.ToString())).ToList();

    var maxCupLabel = cups.Max();
    var minCupLabel = cups.Min();

    var currentCup = cups[0];

    for (int i = 0; i < 100; i++)
    {
        var nextThree = Enumerable.Range(0, 3).Select(x => GetNextAfter(cups, cups.IndexOf(currentCup), true)).ToArray();

        var destinationCup = GetDestinationCup(cups, currentCup, nextThree, maxCupLabel, minCupLabel);

        var destinationCupIdx = cups.IndexOf(destinationCup);

        cups.InsertRange(destinationCupIdx + 1, nextThree);

        currentCup = GetNextAfter(cups, cups.IndexOf(currentCup));
    }

    var solution = string.Empty;

    var nextIdx = cups.IndexOf(1);

    for (int i = 0; i < cups.Count - 1; i++)
    {
        var next = GetNextAfter(cups, nextIdx, false);
        nextIdx = cups.IndexOf(next);
        solution += $"{next}";
    }

    Console.WriteLine($"Part 1: {solution}");

    int GetDestinationCup(List<int> cups, int currentCup, int[] nextThree, int maxCupLabel, int minCupLabel)
    {
        var destinationCup = currentCup;

        do
        {
            destinationCup--;

            if (destinationCup < minCupLabel) destinationCup += maxCupLabel;
        }
        while (nextThree.Contains(destinationCup));

        return destinationCup;
    }

    int GetNextAfter(List<int> cups, int idx, bool remove = false)
    {
        var nextIdx = (idx + 1) % cups.Count;
        var nextItem = cups[nextIdx];
        if (remove) cups.RemoveAt(nextIdx);
        return nextItem;
    }
}

void SolvePart2()
{
    var cupsList = File.ReadAllLines("input.txt")[0].Select(c => int.Parse(c.ToString())).ToList();

    const int minValue = 1;
    const int maxValue = 1_000_000;

    cupsList.AddRange(Enumerable.Range(cupsList.Max() + 1, maxValue - cupsList.Count));

    var cupsLinkedList = new LinkedList<int>(cupsList);

    var nodeMemory = new Dictionary<int, LinkedListNode<int>>();
    
    var tmpNode = cupsLinkedList.First;

    do
    {
        nodeMemory[tmpNode.Value] = tmpNode;
        tmpNode = tmpNode.Next;
    }
    while (tmpNode != null);

    var current = cupsLinkedList.First;
    for (int i = 0; i < 10_000_000; i++)
    {
        List<LinkedListNode<int>> removedNodes = new();

        var tmpCurrent = current;

        for(int j = 0; j < 3; j++)
        {
            tmpCurrent = tmpCurrent.Next ?? tmpCurrent.List.First;
            removedNodes.Add(tmpCurrent);
        }

        foreach (var node in removedNodes)
        {
            cupsLinkedList.Remove(node);
        }

        var destinationValue = current.Value;

        do
        {
            destinationValue--;

            if (destinationValue < minValue) destinationValue = maxValue;
        }
        while (removedNodes.Select(x => x.Value).Contains(destinationValue));

        var destination = nodeMemory[destinationValue];

        foreach (var node in removedNodes)
        {
            cupsLinkedList.AddAfter(destination, node);
            destination = node;
        }

        current = current.Next ?? current.List.First;
    }

    var targetNode = nodeMemory[1];

    long solution = (long)targetNode.Next.Value * (long)targetNode.Next.Next.Value;

    Console.WriteLine($"Part 2: {solution}");
}

SolvePart1();
SolvePart2();
