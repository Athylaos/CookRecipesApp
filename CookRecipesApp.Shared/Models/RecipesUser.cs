using System;
using System.Collections.Generic;

namespace CookRecipesApp.Shared.Models;

public partial class RecipesUser
{
    public Guid RecipesId { get; set; }

    public Guid UsersId { get; set; }

    public bool IsFavorite { get; set; }

    public virtual Recipe Recipes { get; set; } = null!;

    public virtual User Users { get; set; } = null!;
}
