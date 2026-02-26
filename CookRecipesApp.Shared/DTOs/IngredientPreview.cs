using CookRecipesApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Shared.DTOs
{
    public class IngredientPreview
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public Unit DefaultUnit { get; set; }

        public Unit SelectedUnit { get; set; }

        public List<UnitPreviewDto> IngredientUnits { get; set; } = new List<UnitPreviewDto>();

    }
}
