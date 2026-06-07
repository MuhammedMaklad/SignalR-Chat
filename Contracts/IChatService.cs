using SignalRChatApp.Models;

namespace SignalRChatApp.Contracts
{
  public interface IChatService
  {
    Task<List<UserDto>> GetAllUsersAsync(string excludeUserId);
    Task<List<ChatMessageDto>> GetConversationAsync(string userId1, string userId2, string currentUserId);
    Task<List<ChatMessageDto>> GetGroupMessagesAsync(int groupId, string currentUserId);
    Task SaveMessageAsync(ChatMessage message);
  }
}
