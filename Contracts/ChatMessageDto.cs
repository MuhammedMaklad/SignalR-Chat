namespace SignalRChatApp.Contracts
{
  public class ChatMessageDto
  {
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string SenderId { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public bool IsMine { get; set; }
    public int? GroupId { get; set; }
  }
}
