using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalRChatApp.Contracts;
using SignalRChatApp.Models;

namespace SignalRChatApp.Controllers
{
  [Authorize]
  public class GroupsController : Controller
  {
    private readonly IGroupService _groupService;
    private readonly UserManager<AppUser> _userManager;

    public GroupsController(IGroupService groupService, UserManager<AppUser> userManager)
    {
      _groupService = groupService;
      _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
      var currentUser = await _userManager.GetUserAsync(User) ?? throw new InvalidOperationException("User not found");
      var groups = await _groupService.GetUserGroupsAsync(currentUser.Id);
      return View(groups);
    }

    [HttpGet]
    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        ModelState.AddModelError("name", "Group name is required.");
        return View();
      }

      var currentUser = await _userManager.GetUserAsync(User) ?? throw new InvalidOperationException("User not found");
      await _groupService.CreateGroupAsync(name.Trim(), currentUser.Id);

      return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
      var currentUser = await _userManager.GetUserAsync(User) ?? throw new InvalidOperationException("User not found");
      await _groupService.DeleteGroupAsync(id, currentUser.Id);

      return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> AddMember(int id)
    {
      var users = await _groupService.GetAvailableUsersAsync(id);
      ViewBag.GroupId = id;
      return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> AddMember(int groupId, string userId)
    {
      await _groupService.AddUserToGroupAsync(groupId, userId);
      return RedirectToAction("Index");
    }
  }
}
