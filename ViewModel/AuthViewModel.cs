using System.ComponentModel.DataAnnotations;

namespace SignalRChatApp.ViewModel
{
  public class RegisterViewModel
  {
    [Required]
    public string Username { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]  
    public string Password { get; set; }
    [Required]
    [Compare("Password")] 
    public string ConfirmPassword { get; set; }
  }

  public class LoginViewModel
  {
    [Required]
    [EmailAddress]  
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public bool RememberMe { get; set; }
  } 

}
