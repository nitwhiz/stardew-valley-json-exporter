using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StardewValley;

namespace JsonExporter.Model;

[JsonObject(MemberSerialization.OptIn)]
public class Recipe
{
    public readonly List<Ingredient> Ingredients = new();

    [JsonProperty("name")] public string Name;

    [JsonProperty("output")] public RecipeOutput Output;

    [JsonProperty("ingredients")] public Ingredient[] IngredientsAsArray => Ingredients.ToArray();

    [JsonProperty("isCooking")] public bool IsCooking;

    public Recipe(CraftingRecipe recipe)
    {
        Name = recipe.name;
        IsCooking = recipe.isCookingRecipe;

        Output = new RecipeOutput(recipe);

        foreach (var ingredient in recipe.recipeList) Ingredients.Add(new Ingredient(ingredient.Key, ingredient.Value));
    }
}