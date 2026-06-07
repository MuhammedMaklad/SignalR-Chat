using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalRChatApp.Contracts;
using SignalRChatApp.Models;

namespace SignalRChatApp.Controllers
{
  [Authorize]
  public class ChatController : Controller
  {
    private readonly IChatService _chatService;
    private readonly IGroupService _groupService;
    private readonly UserManager<AppUser> _userManager;

    public ChatController(IChatService chatService, IGroupService groupService, UserManager<AppUser> userManager)
    {
      _chatService = chatService;
      _groupService = groupService;
      _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
      var currentUser = await _userManager.GetUserAsync(User) ?? throw new InvalidOperationException("User not found");
      ViewBag.Users = await _chatService.GetAllUsersAsync(currentUser.Id);
      ViewBag.Groups = await _groupService.GetUserGroupsAsync(currentUser.Id);
      return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetConversation(string userId)
    {
      var currentUser = await _userManager.GetUserAsync(User) ?? throw new InvalidOperationException("User not found");
      var messages = await _chatService.GetConversationAsync(currentUser.Id, userId, currentUser.Id);
      return Json(messages);
    }

    [HttpGet]
    public async Task<IActionResult> GetGroupMessages(int groupId)
    {
      var currentUser = await _userManager.GetUserAsync(User) ?? throw new InvalidOperationException("User not found");
      var messages = await _chatService.GetGroupMessagesAsync(groupId, currentUser.Id);
      return Json(messages);
    }
  }
}
