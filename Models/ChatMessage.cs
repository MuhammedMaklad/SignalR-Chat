using System;

namespace SignalRChatApp.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string SenderId { get; set; } = string.Empty;
        public AppUser Sender { get; set; } = null!;

        public string? ReceiverId { get; set; }
        public AppUser? Receiver { get; set; }

        public int? GroupId { get; set; }
        public Group? Group { get; set; }

        public bool IsGroupMessage => GroupId.HasValue;
        public bool IsRead { get; set; }
    }
}
