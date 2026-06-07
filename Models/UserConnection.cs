namespace SignalRChatApp.Models
{
  public class UserConnection
  {
    public int Id { get; set; }
    public string  UserId { get; set; }
    public AppUser User { get; set; } = null!;


    public string ConnectionId { get; set; } = string.Empty;
    public DateTime ConnectedAt { get; set; } = DateTime.Now;
    public DateTime? DisconnectedAt { get; set; }
  }
}
