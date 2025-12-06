
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
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
