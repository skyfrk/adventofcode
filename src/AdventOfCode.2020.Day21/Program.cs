using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt");

var foods = input.Select(line =>
{
    var lineParts = line.Split(" (contains ");
    var ingredients = lineParts[0].Trim().Split(" ");
    var allergens = lineParts[1].Replace(")", string.Empty).Split(", ");
    return new Food(ingredients.ToList(), allergens.ToList());
}).ToArray();

var stateChanged = true;

while (stateChanged)
{
    stateChanged = false;

    foreach (var food in foods)
    {
        foreach (var allergen in food.Allergens)
        {
            var foodsThatContainGivenAllergen = foods.Where(f => f.Allergens.Contains(allergen));
            var commonIngredients = foodsThatContainGivenAllergen
                .OrderBy(f => f.Ingredients.Count)
                .First().Ingredients
                .Where(i => foodsThatContainGivenAllergen.All(f2 => f2.Ingredients.Contains(i)))
                .ToArray();

            if (commonIngredients.Length == 1)
            {
                foreach (var f in foods)
                {
                    f.Ingredients.Remove(commonIngredients[0]);
                }

                stateChanged = true;
            }
        }
    }
}

Console.WriteLine($"Part 1: {foods.Select(f => f.Ingredients.Count).Sum()}");

record Food(List<string> Ingredients, List<string> Allergens);