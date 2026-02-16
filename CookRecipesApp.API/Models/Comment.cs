using System;
using System.Collections.Generic;

namespace CookRecipesApp.API.Models;

public partial class Comment
{
    public Guid RecipeId { get; set; }

    public Guid UserId { get; set; }

    public string? Text { get; set; }

    public short Rating { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
