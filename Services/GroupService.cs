using Microsoft.EntityFrameworkCore;
using SignalRChatApp.Contracts;
using SignalRChatApp.Data;
using SignalRChatApp.Models;

namespace SignalRChatApp.Services
{
  public class GroupService : IGroupService
  {
    private readonly AppDbContext _context;

    public GroupService(AppDbContext context)
    {
      _context = context;
    }

    public async Task<GroupDto> CreateGroupAsync(string name, string ownerId)
    {
      var group = new Group { Name = name, OwnerId = ownerId };
      _context.Groups.Add(group);
      await _context.SaveChangesAsync();

      var userGroup = new UserGroup { UserId = ownerId, GroupId = group.Id };
      _context.UserGroups.Add(userGroup);
      await _context.SaveChangesAsync();

      return new GroupDto
      {
        Id = group.Id,
        Name = group.Name,
        OwnerId = group.OwnerId,
        MemberCount = 1
      };
    }

    public async Task DeleteGroupAsync(int groupId, string ownerId)
    {
      var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == groupId && g.OwnerId == ownerId);

      if (group is not null)
      {
        _context.Groups.Remove(group);
        await _context.SaveChangesAsync();
      }
    }

    public async Task<List<GroupDto>> GetUserGroupsAsync(string userId)
    {
      return await _context.UserGroups
          .Where(ug => ug.UserId == userId)
          .Include(ug => ug.Group)
          .Select(ug => new GroupDto
          {
            Id = ug.Group.Id,
            Name = ug.Group.Name,
            OwnerId = ug.Group.OwnerId,
            MemberCount = ug.Group.GroupUsers.Count
          })
          .ToListAsync();
    }

    public async Task AddUserToGroupAsync(int groupId, string userId)
    {
      var alreadyMember = await _context.UserGroups.AnyAsync(ug => ug.GroupId == groupId && ug.UserId == userId);

      if (!alreadyMember)
      {
        _context.UserGroups.Add(new UserGroup { GroupId = groupId, UserId = userId });
        await _context.SaveChangesAsync();
      }
    }

    public async Task<List<UserDto>> GetAvailableUsersAsync(int groupId)
    {
      var memberIds = await _context.UserGroups
          .Where(ug => ug.GroupId == groupId)
          .Select(ug => ug.UserId)
          .ToListAsync();

      return await _context.Users
          .Where(u => !memberIds.Contains(u.Id))
          .Select(u => new UserDto { Id = u.Id, UserName = u.UserName ?? string.Empty })
          .ToListAsync();
    }
  }
}
