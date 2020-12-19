using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("input.txt");

long sum = 0;

foreach (var line in input)
{
    var tokens = GetTokens(line);
    var tokenTree = ParseTokens(tokens);
    var value = tokenTree.GetValue();
    sum += value;
}

Console.WriteLine($"Part 1: {sum}");

TokenTree ParseTokens(Token[] tokens)
{
    if (tokens.Length == 1)
    {
        return new TokenTree(tokens[0], null, null);
    }

    if (tokens.Length < 3) throw new InvalidOperationException("Can not parse a token tree. Token count invalid.");

    var remainingTokens = tokens.Select(t => t).ToList();

    TokenTree leftTree = null;
    TokenTree nextTree = null;
    Token operatorToken = null;

    while (remainingTokens.Count > 0)
    {
        if (remainingTokens[0].Type == TokenType.Number)
        {
            var numberTokens = new List<Token>();
            var tokenToBeRemovedCount = 0;

            for (int i = 0; i < remainingTokens.Count; i++)
            {
                if (remainingTokens[i].Type == TokenType.Number)
                {
                    numberTokens.Add(remainingTokens[i]);
                    tokenToBeRemovedCount++;
                }
                else break;
            }

            remainingTokens.RemoveRange(0, tokenToBeRemovedCount);

            var number = int.Parse(string.Concat(numberTokens.Select(t => t.Value.ToString())));
            var numberToken = new Token(TokenType.Number, number);
            nextTree = new TokenTree(numberToken, null, null);
        }
        else if (remainingTokens[0].Type == TokenType.BracketOpen)
        {
            var openBracketCount = 1;
            var tokensInBrackets = new List<Token>();
            var tokenIndicesToBeRemovedCount = 1;

            for (int i = 1; i < remainingTokens.Count; i++)
            {
                if (remainingTokens[i].Type == TokenType.BracketOpen) openBracketCount++;
                if (remainingTokens[i].Type == TokenType.BracketClose) openBracketCount--;

                tokenIndicesToBeRemovedCount++;

                if (openBracketCount == 0) break;

                tokensInBrackets.Add(remainingTokens[i]);
            }

            remainingTokens.RemoveRange(0, tokenIndicesToBeRemovedCount);

            nextTree = ParseTokens(tokensInBrackets.ToArray());
        }
        else if (remainingTokens[0].Type is TokenType.Multiplication or TokenType.Addition)
        {
            operatorToken = remainingTokens[0];
            remainingTokens.RemoveAt(0);
        }

        if (nextTree != null && operatorToken != null)
        {
            if (leftTree == null)
            {
                leftTree = nextTree;
                nextTree = null;
                continue;
            }

            leftTree = new TokenTree(operatorToken, leftTree, nextTree);
            nextTree = null;
            operatorToken = null;
        }
    }

    return leftTree;
}

Token[] GetTokens(string line)
{
    var tokens = new List<Token>();

    foreach (var value in line)
    {
        if (value == ' ') continue;

        var tokenType = value switch
        {
            '+' => TokenType.Addition,
            '*' => TokenType.Multiplication,
            '(' => TokenType.BracketOpen,
            ')' => TokenType.BracketClose,
            '0' => TokenType.Number,
            '1' => TokenType.Number,
            '2' => TokenType.Number,
            '3' => TokenType.Number,
            '4' => TokenType.Number,
            '5' => TokenType.Number,
            '6' => TokenType.Number,
            '7' => TokenType.Number,
            '8' => TokenType.Number,
            '9' => TokenType.Number,
            _ => throw new InvalidOperationException("Invalid token.")
        };

        if(tokenType == TokenType.Number)
        {
            tokens.Add(new Token(tokenType, (int)char.GetNumericValue(value)));
        }
        else
        {
            tokens.Add(new Token(tokenType, 0));
        }
    }

    return tokens.ToArray();
}

enum TokenType
{
    Number,
    Addition,
    Multiplication,
    BracketOpen,
    BracketClose
}

record Token(TokenType Type, int Value);

record TokenTree(Token Token, TokenTree LeftChild, TokenTree RightChild)
{
    public long GetValue() => Token.Type switch
        {
            TokenType.Number => Token.Value,
            TokenType.Addition => LeftChild.GetValue() + RightChild.GetValue(),
            TokenType.Multiplication => LeftChild.GetValue() * RightChild.GetValue(),
            _ => throw new InvalidOperationException("Can not get value for given token type.")
        };
}