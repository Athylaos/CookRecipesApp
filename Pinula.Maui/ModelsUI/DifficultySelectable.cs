using CommunityToolkit.Mvvm.ComponentModel;
using Pinula.Shared.Models;

namespace Pinula.ModelsUI
{
    public partial class DifficultySelectable : ObservableObject
    {
        [ObservableProperty]
        private bool isSelected;

        public DifficultyLevel Difficulty { get; set; }
        public string Name => Difficulty.ToString();
    }
}
