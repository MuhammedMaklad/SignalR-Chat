namespace SignalRChatApp.Models
{
  public class UserGroup
  {
    public string UserId { get; set; }
    public int GroupId { get; set; }

    public Group Group { get; set; }
    public AppUser User { get; set; }
  }
}
