﻿using Business.Models;
using Business.Services;
using Domain.Extensions;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using System.Security.Claims;

namespace Presentation.Controllers;

public class AuthController(IAuthService authService, INotificationService notificationService, IUserService userService) : Controller
{
    private readonly IAuthService _authService = authService;
    private readonly INotificationService _notificationService = notificationService;
    private readonly IUserService _userService = userService;


    [Route("auth/signup")]
    public IActionResult SignUp(string returnUrl = "~/")
    {
        ViewBag.ReturnUrl = returnUrl;
        ViewBag.ErrorMessage = "";

        return View();
    }

    [HttpPost]
    [Route("auth/signup")]
    public async Task<IActionResult> SignUp(SignUpViewModel model, string returnUrl = "~/")
    {
        ViewBag.ErrorMessage = null;

        if (!ModelState.IsValid)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ErrorMessage = "";
            return View(model);
        }
        var signUpFormData = model.MapTo<SignUpFormData>();
        var authResult = await _authService.SignUpAsync(signUpFormData);

        if (authResult.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }

        ViewBag.ReturnUrl = returnUrl;
        ViewBag.ErrorMessage = authResult.Error;
        return View(model);
    }


    [Route("auth/login")]
    public IActionResult Login(string returnUrl = "~/")
    {
        ViewBag.ReturnUrl = returnUrl;
        ViewBag.ErrorMessage = "";

        return View();
    }

    [HttpPost]
    [Route("auth/login")]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "~/")
    {
        ViewBag.ReturnUrl = returnUrl;
        ViewBag.ErrorMessage = null;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var signInFormData = model.MapTo<SignInFormData>();
        var authResult = await _authService.SignInAsync(signInFormData);

        if (authResult.Succeeded)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userResult = await _userService.GetUserByIdAsync(userId!);
            var user = userResult.Result;

            if (user != null)
            {

                var notificationFormData = new NotificationFormData
                {
                    NotificationTypeId = 1, // handlar om users, ej projekt
                    NotificationTargetId = 1, //här kan man hämta vilket id som är för admin eller ej ist för att hårdkoda 1
                    Message = $"{user.FirstName} {user.LastName} signed in",
                    Image = !string.IsNullOrEmpty(user.Image) ? $"/images/uploads/{user.Image}" : "/images/profiles/user-template.svg"
                };
                await _notificationService.AddNotificationAsync(notificationFormData);
            }

            return LocalRedirect(returnUrl);
        }

        ViewBag.ErrorMessage = authResult.Error ;
        return View(model);
    }

    [Route("auth/logout")]
    public async Task<IActionResult> Logout()
    {
       var result = await _authService.SignOutAsync();
        return LocalRedirect("~/");
    }
}
