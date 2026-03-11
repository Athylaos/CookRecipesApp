using System;
using System.Collections.Generic;
using System.Text;

namespace CookRecipesApp.Shared.DTOs
{
    public class CommentPreview
    {
        public string Text { get; set; } = string.Empty;
        public short Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserSurname { get; set; } = string.Empty;
    }
}
