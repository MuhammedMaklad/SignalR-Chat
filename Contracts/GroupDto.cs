namespace SignalRChatApp.Contracts
{
  public class GroupDto
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string OwnerId { get; set; } = string.Empty;
    public int MemberCount { get; set; }
  }
}
