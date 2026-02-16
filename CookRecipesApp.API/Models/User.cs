using System;
using System.Collections.Generic;

namespace CookRecipesApp.API.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public DateTime UserCreated { get; set; }

    public string? Role { get; set; }

    public string? AvatarUrl { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
