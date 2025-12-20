using CookRecipesApp.Model.Ingredient;
using CookRecipesApp.Model.Recepie;
using CookRecipesApp.Model.Category;
using CookRecipesApp.Model.User;
using CookRecipesApp.Service;
using Microsoft.Extensions.DependencyInjection;
using SQLite;

namespace CookRecipesApp
{
    public partial class App : Application
    {

        private readonly SQLiteConnectionFactory _connectionFactory;


        public App(SQLiteConnectionFactory connectionFactory)
        {
            InitializeComponent();

            _connectionFactory = connectionFactory;

        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override async void OnStart()
        {
            ISQLiteAsyncConnection database = _connectionFactory.CreateConnection();

            await database.CreateTableAsync<UserDbModel>();
            await database.CreateTableAsync<UnitDbModel>();
            await database.CreateTableAsync<IngredientDbModel>();
            await database.CreateTableAsync<IngredientUnitDbModel>();
            await database.CreateTableAsync<RecepieDbModel>();
            await database.CreateTableAsync<CategoryDbModel>();
            await database.CreateTableAsync<CommentDbModel>();
            await database.CreateTableAsync<RecepieCategoryDbModel>();
            await database.CreateTableAsync<RecepieIngredientDbModel>();
            await database.CreateTableAsync<RecepieStepDbModel>();


            DatabaseSeederService ds = new(database);
            if (await database.Table<CategoryDbModel>().CountAsync() == 0)
            {
                await ds.SeedCategoriesAsync();
            }

            await ds.SeedDataAsync();



            base.OnStart();
        }



    }
}