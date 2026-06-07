using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;

namespace SignalRChatApp.Models
{
  public class AppUser : IdentityUser
  {

    public ICollection<UserConnection> UserConnections { get; set; } = new HashSet<UserConnection>();
    public ICollection<Group> OwnedGroups { get; } = new List<Group>();
    public ICollection<UserGroup> UserGroups { get; set; } = new HashSet<UserGroup>();

    public ICollection<ChatMessage> SentMessages { get; set; } = new HashSet<ChatMessage>();
    public ICollection<ChatMessage> ReceivedMessages { get; set; } = new HashSet<ChatMessage>();
  }
}
