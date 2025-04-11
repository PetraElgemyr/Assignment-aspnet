using Business.Handlers;
using Business.Models;
using Business.Services;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using System.Security.Claims;

namespace Presentation.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController(IUserService userService, IAzureImageHandler azureImageHandler) : Controller
{

    private readonly IUserService _userService = userService;
    private readonly IAzureImageHandler _azureImageHandler = azureImageHandler;

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

        var imageUri = await _azureImageHandler.UploadFileAsync(model.Image!);
        var userFormData = model.MapTo<AddMemberFormData>();
        userFormData.Image = imageUri;

        var result = await _userService.CreateMemberAsync(userFormData);
        if (result.Succeeded)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var userDislayName = await _userService.GetDisplayNameAsync(userId);

            var notificationFormData = new NotificationFormData
            {
                NotificationTypeId = 1, //projects
                NotificationTargetId = 2, //admin
                Message = $"New member {model.FirstName} {model.FirstName} was added by {userDislayName}",
                Image = !string.IsNullOrEmpty(imageUri) ? imageUri : "/images/profiles/user-template.svg" 
            };
        }


        return result.StatusCode switch
        {
            201 => Ok(),
            400 => BadRequest(result.Error),
            409 => Conflict(),
            _ => Problem(),
        };

    }
}
