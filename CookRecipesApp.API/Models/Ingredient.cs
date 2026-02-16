using System;
using System.Collections.Generic;

namespace CookRecipesApp.API.Models;

public partial class Ingredient
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid DefaultUnit { get; set; }

    public decimal Calories { get; set; }

    public decimal Proteins { get; set; }

    public decimal Fats { get; set; }

    public decimal Carbohydrates { get; set; }

    public decimal Fiber { get; set; }

    public virtual Unit DefaultUnitNavigation { get; set; } = null!;

    public virtual ICollection<IngredientUnit> IngredientUnits { get; set; } = new List<IngredientUnit>();

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}
