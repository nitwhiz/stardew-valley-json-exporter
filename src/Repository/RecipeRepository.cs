using System;
using System.Collections.Generic;
using System.Linq;
using JsonExporter.Model;
using Newtonsoft.Json;
using StardewValley;

namespace JsonExporter.Repository;

public class RecipeRepository : Repository<RecipeRepository, Recipe>
{
    private static readonly Dictionary<string, Recipe> Recipes = new();

    [JsonProperty("recipes")] private static Recipe[] RecipesAsArray => Recipes.Values.ToArray();

    [JsonProperty("version")] private static string _version = DateTime.Now.ToString("u");

    public override void Populate()
    {
        Recipes.Clear();

        foreach (var value in CraftingRecipe.cookingRecipes.Keys)
        {
            var recipe = new Recipe(new CraftingRecipe(value, true));
            Recipes[recipe.Name] = recipe;
        }

        foreach (var value in CraftingRecipe.craftingRecipes.Keys)
        {
            var recipe = new Recipe(new CraftingRecipe(value, false));
            Recipes[recipe.Name] = recipe;
        }
    }

    public override List<Recipe> GetAll()
    {
        return Recipes.Values.ToList();
    }
}