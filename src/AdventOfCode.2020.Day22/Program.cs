using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllText("input.txt");

Queue<int> player1Queue = new();
Queue<int> player2Queue = new();

foreach (var num in input.Split("\r\n\r\n")[0].Replace("Player 1:\r\n", string.Empty).Split("\r\n").Select(int.Parse))
{
    player1Queue.Enqueue(num);
}

foreach (var num in input.Split("\r\n\r\n")[1].Replace("Player 2:\r\n", string.Empty).Split("\r\n").Select(int.Parse))
{
    player2Queue.Enqueue(num);
}

while(player1Queue.Count != 0 && player2Queue.Count != 0)
{
    var player1num = player1Queue.Dequeue();
    var player2num = player2Queue.Dequeue();

    if(player1num > player2num)
    {
        player1Queue.Enqueue(player1num);
        player1Queue.Enqueue(player2num);
    }

    if(player1num < player2num)
    {
        player2Queue.Enqueue(player2num);
        player2Queue.Enqueue(player1num);
    }

    // discard the numbers if equal?
}


long score = -1;

if(player2Queue.Count == 0)
{
    score = GetScore(player1Queue);
}

if (player1Queue.Count == 0)
{
    score = GetScore(player2Queue);
}

Console.WriteLine($"Part 1: {score}");

long GetScore(Queue<int> deck)
{
    long score = 0;

    for(int pos = deck.Count; pos > 0; pos--)
    {
        score += pos * deck.Dequeue();
    }

    return score;
}