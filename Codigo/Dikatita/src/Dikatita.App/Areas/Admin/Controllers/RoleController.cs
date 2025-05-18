using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dikatita.App.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
[Route("roles/[action]")]

public class RoleController : BaseController
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public RoleController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet("listagem-usuarios")]
    public IActionResult Index()
    {
        var users = _userManager.Users.ToList();
        return View(users);
    }

    [HttpGet("atribuir-roles")]
    public async Task<IActionResult> ManageRoles(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        var userRoles = await _userManager.GetRolesAsync(user);
        var allRoles = _roleManager.Roles.ToList();

        ViewBag.UserId = userId;
        ViewBag.UserRoles = userRoles;
        ViewBag.AllRoles = allRoles;

        return View();
    }

    [HttpPost("atribuir-roles")]
    public async Task<IActionResult> UpdateRoles(string userId, List<string> roles)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        var userRoles = await _userManager.GetRolesAsync(user);
        var result = await _userManager.RemoveFromRolesAsync(user, userRoles);
        if (!result.Succeeded)
            return BadRequest("Erro ao remover roles.");

        result = await _userManager.AddToRolesAsync(user, roles);
        if (!result.Succeeded)
            return BadRequest("Erro ao adicionar roles.");

        return RedirectToAction(nameof(Index));
    }
}
