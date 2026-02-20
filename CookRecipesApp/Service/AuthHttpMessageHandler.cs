using CookRecipesApp.View;
using System.Diagnostics;
using System.Net.Http.Headers;

public class AuthHttpMessageHandler : DelegatingHandler
{
    public AuthHttpMessageHandler()
    {
        
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await SecureStorage.Default.GetAsync("auth_token");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            SecureStorage.Default.Remove("auth_token");
            Debug.WriteLine("User is unauthorized for this");
            Shell.Current.GoToAsync(nameof(LoginPage));
        }

        return response;
    }
}