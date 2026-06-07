namespace SignalRChatApp.Contracts
{
  public class AuthResult
  {
    public bool Succeeded { get; set; }
    public IEnumerable<string> Errors { get; set; } = [];
  }
}
