using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model.Recepie
{
    public partial class RecepieStep : ObservableObject
    {
        public int Id { get; set; }
        public int RecepieId { get; set; }
        public string ContentText { get; set; } = string.Empty;
        public int Order { get; set; }

    }
}
