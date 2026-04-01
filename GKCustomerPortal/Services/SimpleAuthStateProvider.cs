using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace GKCustomerPortal.Services;

public class SimpleAuthStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());

    public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
        Task.FromResult(new AuthenticationState(_currentUser));

    public void MarkUserAsAuthenticated(string email)
    {
        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "apiauth");
        _currentUser = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}