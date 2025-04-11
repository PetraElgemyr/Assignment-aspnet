using Business.Models;
using Business.Services;
using Domain.Extensions;
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

      
            var model = new MembersViewModel
            {
                Members = userResults.Result!,
                AddUserViewModel = new AddUserViewModel()
            };
            return View(model);
    }



    [HttpPost]
    [Route("admin/members")]
    public async Task<IActionResult> Add(AddUserViewModel model)
    {
        ViewBag.ErrorMessage = null;

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return BadRequest(new { errors });
        }
        var userFormData = model.MapTo<AddMemberFormData>();

        var result = await _userService.CreateMemberAsync(userFormData);
        return result.StatusCode switch
        {
            201 => Ok(),
            400 => BadRequest(result.Error),
            409 => Conflict(),
            _ => Problem(),
        };

    }
}
