using CommunityToolkit.Mvvm.ComponentModel;
using CookRecipesApp.Shared.Models;

namespace CookRecipesApp.ModelsUI
{
    public partial class DifficultySelectable : ObservableObject
    {
        [ObservableProperty]
        private bool isSelected;

        public DifficultyLevel Difficulty { get; set; }
        public string Name => Difficulty.ToString();
    }
}
