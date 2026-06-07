using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRChatApp.Data;
using SignalRChatApp.Models;

namespace SignalRChatApp.Hubs
{
  [Authorize]
  public class ChatHub : Hub
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _context;

    public ChatHub(UserManager<AppUser> userManager, AppDbContext context)
    {
      _userManager = userManager;
      _context = context;
    }

    public async Task SendMessageToUser(string receiverId, string content)
    {
      var sender = await _userManager.GetUserAsync(Context.User!);
      if (sender is null) return;

      var message = new ChatMessage
      {
        Content = content,
        Timestamp = DateTime.UtcNow,
        SenderId = sender.Id,
        ReceiverId = receiverId
      };

      _context.ChatMessages.Add(message);
      await _context.SaveChangesAsync();

      var messageDto = new
      {
        id = message.Id,
        content = message.Content,
        timestamp = message.Timestamp,
        senderId = sender.Id,
        senderName = sender.UserName ?? string.Empty,
        isMine = true
      };

      var receiverDto = new
      {
        id = message.Id,
        content = message.Content,
        timestamp = message.Timestamp,
        senderId = sender.Id,
        senderName = sender.UserName ?? string.Empty,
        isMine = false
      };

      await Clients.Caller.SendAsync("ReceiveMessage", messageDto);

      if (receiverId != sender.Id)
      {
        await Clients.User(receiverId).SendAsync("ReceiveMessage", receiverDto);
      }
    }

    public async Task SendMessageToGroup(int groupId, string content)
    {
      var sender = await _userManager.GetUserAsync(Context.User!);
      if (sender is null) return;

      var message = new ChatMessage
      {
        Content = content,
        Timestamp = DateTime.UtcNow,
        SenderId = sender.Id,
        GroupId = groupId
      };

      _context.ChatMessages.Add(message);
      await _context.SaveChangesAsync();

      var messageDto = new
      {
        id = message.Id,
        content = message.Content,
        timestamp = message.Timestamp,
        senderId = sender.Id,
        senderName = sender.UserName ?? string.Empty,
        groupId = groupId
      };

      await Clients.Group($"g_{groupId}").SendAsync("ReceiveGroupMessage", messageDto);
    }

    public async Task JoinGroup(int groupId)
    {
      await Groups.AddToGroupAsync(Context.ConnectionId, $"g_{groupId}");
    }

    public async Task LeaveGroup(int groupId)
    {
      await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"g_{groupId}");
    }

    public override async Task OnConnectedAsync()
    {
      var user = await _userManager.GetUserAsync(Context.User!);
      if (user is not null)
      {
        _context.UserConnections.Add(new UserConnection
        {
          UserId = user.Id,
          ConnectionId = Context.ConnectionId,
          ConnectedAt = DateTime.UtcNow
        });

        var groupIds = await _context.UserGroups
            .Where(ug => ug.UserId == user.Id)
            .Select(ug => ug.GroupId)
            .ToListAsync();

        foreach (var groupId in groupIds)
        {
          await Groups.AddToGroupAsync(Context.ConnectionId, $"g_{groupId}");
        }

        await _context.SaveChangesAsync();
      }

      await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
      var connection = await _context.UserConnections
          .OrderByDescending(uc => uc.ConnectedAt)
          .FirstOrDefaultAsync(uc => uc.ConnectionId == Context.ConnectionId);

      if (connection is not null)
      {
        connection.DisconnectedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
      }

      await base.OnDisconnectedAsync(exception);
    }
  }
}
