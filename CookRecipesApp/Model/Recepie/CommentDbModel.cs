using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Model.Recepie
{
    public class CommentDbModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public int RecepieId { get; set; }
        [NotNull]
        public int UserId { get; set; }
        [NotNull]
        public string Text { get; set; } = string.Empty;
        [NotNull]
        public string CreatedAt { get; set; } = string.Empty;
    }
}
