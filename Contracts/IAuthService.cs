using SignalRChatApp.ViewModel;

namespace SignalRChatApp.Contracts
{
  public interface IAuthService
  {
    Task<AuthResult> RegisterAsync(RegisterViewModel model);
    Task<AuthResult> LoginAsync(LoginViewModel model);
    Task LogoutAsync();
  }
}
