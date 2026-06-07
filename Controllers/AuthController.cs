using Microsoft.AspNetCore.Mvc;
using SignalRChatApp.Contracts;
using SignalRChatApp.ViewModel;

namespace SignalRChatApp.Controllers
{
  public class AuthController : Controller
  {
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
      _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
      if (!ModelState.IsValid)
        return View(model);

      var result = await _authService.LoginAsync(model);

      if (!result.Succeeded)
      {
        foreach (var error in result.Errors)
          ModelState.AddModelError(string.Empty, error);

        return View(model);
      }

      return RedirectToAction("Index", "Chat");
    }

    [HttpGet]
    public IActionResult Register()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
      if (!ModelState.IsValid)
        return View(model);

      var result = await _authService.RegisterAsync(model);

      if (!result.Succeeded)
      {
        foreach (var error in result.Errors)
          ModelState.AddModelError(string.Empty, error);

        return View(model);
      }

      return RedirectToAction("Login");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
      await _authService.LogoutAsync();
      return RedirectToAction("Index", "Home");
    }
  }
}
