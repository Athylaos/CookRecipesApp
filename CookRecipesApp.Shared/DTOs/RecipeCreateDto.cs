using CookRecipesApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Shared.DTOs
{
    public class RecipeCreateDto
    {
        public string Title {  get; set; }

        public string PhotoUrl { get; set; } = "default_recipe_picture.png";

        public short CookingTime { get; set; }

        public short ServingsAmount { get; set; }

        public Guid ServingUnit { get; set; }

        public short Difficulty { get; set; }

        public List<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

        public List<RecipeStep> RecipeSteps { get; set; } = new List<RecipeStep>();

        public List<Guid> CategoriesIds { get; set; } = new List<Guid>();

    }
}
