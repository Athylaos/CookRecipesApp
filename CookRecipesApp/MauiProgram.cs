using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm;
using Microsoft.Extensions.Logging;
using CookRecipesApp.Service;
using CookRecipesApp.View;
using CookRecipesApp.ViewModel;

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




            return builder.Build();
        }
    }
}
