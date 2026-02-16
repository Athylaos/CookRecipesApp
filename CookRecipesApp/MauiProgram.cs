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
            builder.Services.AddSingleton<IIngredientService, IngredientService>();
            builder.Services.AddSingleton<IRecipeService, RecipeService>();
            builder.Services.AddSingleton<ICategoryService, CategoryService>();
            builder.Services.AddSingleton<IUserService, UserService>();

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


            return builder.Build();
        }
    }
}
