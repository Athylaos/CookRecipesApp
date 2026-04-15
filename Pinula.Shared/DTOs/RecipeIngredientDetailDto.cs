using System;
using System.Collections.Generic;
using System.Text;

namespace Pinula.Shared.DTOs
{
    public class RecipeIngredientDetailDto
    {
        public decimal Quantity { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
    }
}
