using System;
using System.Collections.Generic;

namespace CookRecipesApp.API.Models;

public partial class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string PictureUrl { get; set; } = null!;

    public short SortOrder { get; set; }

    public Guid? ParentCategory { get; set; }

    public virtual ICollection<Category> InverseParentCategoryNavigation { get; set; } = new List<Category>();

    public virtual Category? ParentCategoryNavigation { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
