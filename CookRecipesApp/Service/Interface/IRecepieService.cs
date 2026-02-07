using CookRecipesApp.Model.Recepie;

namespace CookRecipesApp.Service.Interface
{
    public interface IRecepieService
    {
        public Task SaveRecepieAsync(Recepie recepie);
        public Task<Recepie> GetRecepieAsync(int id);
        public Task UpdateRecepieAsync(Recepie recepie);
        public Task DeleteRecepieAsync(int id);

        public Task<List<Recepie>> GetRecepiesAsync(int amount);

        public Task ChangeFavoriteAsync(int recepieId, int userId);
        public Task<bool> IsFavoriteAsync(int recepieId, int userId);

        public Task<bool> UserCommentedAsync(int recepieId, int  userId);
        public Task<(float, int)> PostCommentAsync(Comment comment);
        public Task<Comment?> GetCommentByUserAndRecepieAsync(int  recepieId, int userId);
        public Task<(float, int)> DeleteCommentByUserAndRecepieAsync(int recepieId, int userId);

    }
}
