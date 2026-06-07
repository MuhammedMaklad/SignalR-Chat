namespace SignalRChatApp.Contracts
{
  public class UnreadCountDto
  {
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public int? GroupId { get; set; }
    public string? GroupName { get; set; }
    public int Count { get; set; }
  }
}
