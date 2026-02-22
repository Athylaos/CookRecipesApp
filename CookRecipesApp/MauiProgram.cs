using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CookRecipesApp.Service.Services;
using CookRecipesApp.View;
using CookRecipesApp.View.Popups;
using CookRecipesApp.ViewModel;
using CookRecipesApp.ViewModel.Popups;
using CookRecipesApp.Service.Interface;
using Microsoft.Extensions.Logging;
using Sharpnado.MaterialFrame;
using Sharpnado.Shades;
using UraniumUI;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;


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
                .UseSharpnadoMaterialFrame(loggerEnable: false)
                .UseUraniumUIBlurs()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Alegreya-VariableFont_wght.ttf", "Alegreya");
                    fonts.AddFont("Nunito-VariableFont_wght.ttf", "Nunito");
                });

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("Placeholder", (h, v) =>
            {
#if ANDROID
                h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
            });
            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("Placeholder", (h, v) =>
            {
#if ANDROID
                h.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddTransient<AuthHttpMessageHandler>();

            builder.Services.AddHttpClient("CookApi", client =>
            {
                client.BaseAddress = new Uri("https://10.0.1.160:7141/api/");
            })
            .AddHttpMessageHandler<AuthHttpMessageHandler>()
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
#if DEBUG
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
#endif
                return handler;
            });




            builder.Services.AddSingleton<IIngredientService>(sp => new IngredientService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("CookApi")));
            builder.Services.AddSingleton<IRecipeService>(sp => new RecipeService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("CookApi")));
            builder.Services.AddSingleton<ICategoryService>(sp => new CategoryService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("CookApi")));
            builder.Services.AddSingleton<IUserService>(sp => new UserService(sp.GetRequiredService<IHttpClientFactory>().CreateClient("CookApi")));

            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddTransient<LoginViewModel>();

            builder.Services.AddSingleton<RegisterPage>();
            builder.Services.AddTransient<RegisterViewModel>();

            builder.Services.AddSingleton<RecipesMainPage>();
            builder.Services.AddTransient<RecipesMainViewModel>();

            builder.Services.AddSingleton<TestPage>();
            builder.Services.AddTransient<TestViewModel>();

            builder.Services.AddSingleton<AddRecipePage>();
            builder.Services.AddTransient<AddRecipeViewModel>();

            builder.Services.AddSingleton<AddIngredientPopup>();
            builder.Services.AddTransient<AddIngredientPopupViewModel>();

            builder.Services.AddSingleton<RecipesCategoryPage>();
            builder.Services.AddTransient<RecipesCategoryViewModel>();

            builder.Services.AddSingleton<RecipeDetailsPage>();
            builder.Services.AddTransient<RecipeDetailsViewModel>();

                        
            try
            {
                var app = builder.Build();
                return app;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
    }
}
