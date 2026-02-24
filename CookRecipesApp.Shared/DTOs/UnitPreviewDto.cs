using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Shared.DTOs
{
    public class UnitPreviewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal ConversionFactor { get; set; }
    }
}
