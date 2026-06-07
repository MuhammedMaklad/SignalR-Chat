namespace SignalRChatApp.Contracts
{
  public interface IGroupService
  {
    Task<GroupDto> CreateGroupAsync(string name, string ownerId);
    Task DeleteGroupAsync(int groupId, string ownerId);
    Task<List<GroupDto>> GetUserGroupsAsync(string userId);
    Task AddUserToGroupAsync(int groupId, string userId);
    Task<List<UserDto>> GetAvailableUsersAsync(int groupId);
  }
}
