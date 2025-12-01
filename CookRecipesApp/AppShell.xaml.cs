using CookRecipesApp.View;
using CookRecipesApp.View.Account;

namespace CookRecipesApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));


        }
    }
}
