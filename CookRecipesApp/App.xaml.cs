using CookRecipesApp.Shared.Models;
using CookRecipesApp.Service;
using Microsoft.Extensions.DependencyInjection;
using SQLite;
using System.Diagnostics;

namespace CookRecipesApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override async void OnStart()
        {   
            
            //DatabaseSeederService ds = new(new SQLiteConnectionFactory().CreateConnection());
            //await ds.ResetDatabaseAsync();
            //await ds.SeedCompleteRecipesAsync();

            Debug.WriteLine("Seeding done");


            base.OnStart();
        }



    }
}