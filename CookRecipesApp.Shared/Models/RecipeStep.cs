using System;
using System.Collections.Generic;

namespace CookRecipesApp.Shared.Models;

public partial class RecipeStep
{
    public Guid Id { get; set; }

    public Guid RecipeId { get; set; }

    public string Description { get; set; } = null!;

    public short StepNumber { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;
}
