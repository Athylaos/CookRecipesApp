using System;
using System.Collections.Generic;

namespace CookRecipesApp.Shared.Models;

public partial class IngredientUnit
{
    public Guid UnitId { get; set; }

    public Guid IngredientId { get; set; }

    public decimal ToDefaultUnit { get; set; }

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual Unit Unit { get; set; } = null!;
}
