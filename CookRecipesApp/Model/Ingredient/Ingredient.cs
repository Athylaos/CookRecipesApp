
namespace CookRecipesApp.Model.Ingredient
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public UnitDbModel DefaultUnit { get; set; } = new();
        public List<IngredientUnitInfo> PossibleUnits { get; set; } = new();

        public float Calories { get; set; }
        public float Proteins { get; set; }
        public float Fats { get; set; }
        public float Carbohydrates { get; set; }
        public float Fiber { get; set; }

        public class IngredientUnitInfo
        {
            public UnitDbModel Unit { get; set; }
            public float ConversionFactor { get; set; }
        }

    }
}
