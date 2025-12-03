using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm;
using CookRecipesApp.Service;
using CookRecipesApp.View;
using CookRecipesApp.ViewModel;
using Microsoft.Extensions.Logging;
using Sharpnado.Shades;
using UraniumUI;

namespace CookRecipesApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitCore()
                .UseSharpnadoShadows()
                .UseUraniumUIBlurs()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Alegreya-VariableFont_wght.ttf", "Alegreya");
                    fonts.AddFont("Nunito-VariableFont_wght.ttf", "Nunito");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<SQLiteConnectionFactory>();
            builder.Services.AddSingleton<IIngredientsService, IngredientsService>();
            builder.Services.AddSingleton<IRecepiesService,RecepiesService>();

            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddTransient<LoginViewModel>();

            builder.Services.AddSingleton<RegisterPage>();
            builder.Services.AddTransient<RegisterViewModel>();

            builder.Services.AddSingleton<TestPage>();
            builder.Services.AddTransient<TestViewModel>();


            return builder.Build();
        }
    }
}
