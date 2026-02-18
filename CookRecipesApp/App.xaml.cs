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
            //return new Window(new ContentPage { Content = new Label { Text = "DI ok" } });
        }

        protected override async void OnStart()
        {   

            base.OnStart();
        }



    }
}