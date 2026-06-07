using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalRChatApp.Contracts;
using SignalRChatApp.Data;
using SignalRChatApp.Models;

namespace SignalRChatApp.Services
{
  public class ChatService : IChatService
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _context;

    public ChatService(UserManager<AppUser> userManager, AppDbContext context)
    {
      _userManager = userManager;
      _context = context;
    }

    public async Task<List<UserDto>> GetAllUsersAsync(string excludeUserId)
    {
      return await _userManager.Users
          .Where(u => u.Id != excludeUserId)
          .Select(u => new UserDto { Id = u.Id, UserName = u.UserName ?? string.Empty })
          .ToListAsync();
    }

    public async Task<List<ChatMessageDto>> GetConversationAsync(string userId1, string userId2, string currentUserId)
    {
      return await _context.ChatMessages
          .Where(m => !m.IsGroupMessage &&
                      ((m.SenderId == userId1 && m.ReceiverId == userId2) ||
                       (m.SenderId == userId2 && m.ReceiverId == userId1)))
          .OrderBy(m => m.Timestamp)
          .Select(m => new ChatMessageDto
          {
            Id = m.Id,
            Content = m.Content,
            Timestamp = m.Timestamp,
            SenderId = m.SenderId,
            SenderName = m.Sender.UserName ?? string.Empty,
            IsMine = m.SenderId == currentUserId
          })
          .ToListAsync();
    }

    public async Task<List<ChatMessageDto>> GetGroupMessagesAsync(int groupId, string currentUserId)
    {
      return await _context.ChatMessages
          .Where(m => m.GroupId == groupId)
          .OrderBy(m => m.Timestamp)
          .Select(m => new ChatMessageDto
          {
            Id = m.Id,
            Content = m.Content,
            Timestamp = m.Timestamp,
            SenderId = m.SenderId,
            SenderName = m.Sender.UserName ?? string.Empty,
            IsMine = m.SenderId == currentUserId,
            GroupId = m.GroupId
          })
          .ToListAsync();
    }

    public async Task SaveMessageAsync(ChatMessage message)
    {
      _context.ChatMessages.Add(message);
      await _context.SaveChangesAsync();
    }
  }
}
