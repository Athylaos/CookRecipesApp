using CookRecipesApp.Model;
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

            await database.CreateTableAsync<UserDbModel>(); // save to call even if table exists

            


            base.OnStart();
        }
    }
}