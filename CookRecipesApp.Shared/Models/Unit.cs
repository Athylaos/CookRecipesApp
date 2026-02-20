using System;
using System.Collections.Generic;

namespace CookRecipesApp.Shared.Models;

public partial class Unit
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsServingUnit { get; set; }

    public virtual ICollection<IngredientUnit> IngredientUnits { get; set; } = new List<IngredientUnit>();

    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
