
using CookRecipesApp.Model.Recepie;

namespace CookRecipesApp.Model.Ingredient
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public UnitDbModel DefaultUnit { get; set; } = new();
        public List<IngredientUnitInfo> PossibleUnits { get; set; } = new();

        public Nutritions Nutritions { get; set; } = new();

        public class IngredientUnitInfo
        {
            public UnitDbModel Unit { get; set; }
            public float ConversionFactor { get; set; }
        }

    }
}
