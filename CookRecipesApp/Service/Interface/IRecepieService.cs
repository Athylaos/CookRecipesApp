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

    }
}
