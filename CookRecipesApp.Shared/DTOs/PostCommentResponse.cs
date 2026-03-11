namespace CookRecipesApp.Shared.DTOs
{
    public class PostCommentResponse
    {
        public Guid RecipeId { get; set; }
        public Guid UserId { get; set; }
        public string? Text { get; set; } = string.Empty;
        public short Rating { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserSurname { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public decimal NewAverageRating { get; set; }
        public int NewUsersRatedCount { get; set; }
    }
}
