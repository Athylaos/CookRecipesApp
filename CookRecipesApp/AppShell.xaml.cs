using CookRecipesApp.View;

namespace CookRecipesApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

            Routing.RegisterRoute(nameof(TestPage), typeof(TestPage));
            
            Routing.RegisterRoute(nameof(AddRecepiePage), typeof(AddRecepiePage));


        }
    }
}
