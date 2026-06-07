namespace SignalRChatApp.Models
{
  public class Group
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public string OwnerId { get; set; }
    public AppUser Owner { get; set; }

    public ICollection<UserGroup> GroupUsers { get; set; } = new HashSet<UserGroup>();
    public ICollection<ChatMessage> Messages { get; set; } = new HashSet<ChatMessage>();
  }
}
