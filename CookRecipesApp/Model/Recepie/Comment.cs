
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model.Recepie
{
    public class Comment
    {
        public int Id { get; set; }
        public int RecepieId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public float Rating { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
