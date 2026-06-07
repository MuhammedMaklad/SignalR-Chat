using Microsoft.AspNetCore.Identity;
using SignalRChatApp.Contracts;
using SignalRChatApp.Models;
using SignalRChatApp.ViewModel;

namespace SignalRChatApp.Services
{
  public class AuthService : IAuthService
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
      _userManager = userManager;
      _signInManager = signInManager;
    }

    public async Task<AuthResult> RegisterAsync(RegisterViewModel model)
    {
      var user = new AppUser { UserName = model.Username, Email = model.Email };
      var result = await _userManager.CreateAsync(user, model.Password);

      if (!result.Succeeded)
        return new AuthResult { Succeeded = false, Errors = result.Errors.Select(e => e.Description) };

      return new AuthResult { Succeeded = true };
    }

    public async Task<AuthResult> LoginAsync(LoginViewModel model)
    {
      var user = await _userManager.FindByEmailAsync(model.Email);
      if (user is null)
        return new AuthResult { Succeeded = false, Errors = ["Invalid email or password."] };

      var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

      if (!result.Succeeded)
        return new AuthResult { Succeeded = false, Errors = ["Invalid email or password."] };

      return new AuthResult { Succeeded = true };
    }

    public async Task LogoutAsync()
    {
      await _signInManager.SignOutAsync();
    }
  }
}
