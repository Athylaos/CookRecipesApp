using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Shared.DTOs
{
    public class RecipeFilterParametrs
    {
        public string? SearchTerm { get; set; }
        public Guid? CategoryId { get; set; }
        public int? MaxCookingTime { get; set; }
        public int? MinRating { get; set; }
        public int? MaxDifficulty { get; set; }
        public bool OnlyFavorites { get; set; } = false;
        public int Amount { get; set; } = 10;
    }
}
