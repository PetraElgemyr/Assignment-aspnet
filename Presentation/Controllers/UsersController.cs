using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController(IUserService userService) : Controller
{

    private readonly IUserService _userService = userService;

    [Route("admin/members")]
    public async Task<IActionResult> Index()
    {
        ViewBag.ErrorMessage = "";

        var userResults = await _userService.GetUsersAsync();

        if (userResults.Succeeded)
        {
            var model = new MembersViewModel
            {
                Members = userResults.Result!
            };
            return View(model);
        }


        return View();
    }
}
