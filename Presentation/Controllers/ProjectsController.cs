using Business.Models;
using Business.Services;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Presentation.Controllers;

[Authorize]
public class ProjectsController(IProjectService projectService, IClientService clientService, IStatusService statusService, IUserService userService) : Controller
{

    private readonly IProjectService _projectService = projectService;
    private readonly IClientService _clientService = clientService;
    private readonly IStatusService _statusService = statusService;
    private readonly IUserService _userService = userService;

    [Route("admin/projects")]
    public async Task<IActionResult> Index()
    {
        var projectResult = await _projectService.GetProjectsAsync();
        var clientResult = await _clientService.GetClientsAsync();
        var statusResult = await _statusService.GetStatusesAsync();
        var userResult = await _userService.GetUsersAsync();

        var projectViewModels = new List<ProjectViewModel>();
        if (projectResult.Result!.Any())
        {
            foreach (var project in projectResult.Result!)
            {
                var vm = project.MapTo<ProjectViewModel>();
                vm.Client = clientResult.Result!.FirstOrDefault(x => x.Id == project.ClientId)!;
                vm.Status = statusResult.Result!.FirstOrDefault(x => x.Id == project.StatusId)!;
                vm.User = userResult.Result!.FirstOrDefault(x => x.Id == project.UserId)!;
                projectViewModels.Add(vm);
            }
        }

        var model = new ProjectsViewModel
        {
            Projects = projectViewModels,
            Statuses = statusResult.Result ?? [],
            AddProjectViewModel = new AddProjectViewModel
            {
                Users = await GetUsersSelectListAsync(),
                Clients = await GetClientsSelectListAsync(),
            },
            EditProjectViewModel = new UpdateProjectViewModel
            {
                Users = await GetUsersSelectListAsync(),
                Clients = await GetClientsSelectListAsync(),
                Statuses = await GetStatusesSelectListAsync()
            },
        };

        return View(model);
    }



    #region Add

    [HttpPost]
    [Route("admin/projects")]
    public async Task<IActionResult> Add(AddProjectViewModel model)
    {

        ViewBag.ErrorMessage = null;

        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return BadRequest(new { errors });
        }
        
        var addProjectFormData = model.MapTo<AddProjectFormData>();
        var result = await _projectService.CreateProjectAsync(addProjectFormData);
        return result.StatusCode switch
        {
            201 => Ok(),
            400 => BadRequest(result.Error),
            409 => Conflict(),
            _ => Problem(),
        };
     


    }
    #endregion




    [HttpPut]
    [Route("admin/projects")]
    public async Task<IActionResult> Update(UpdateProjectViewModel model)
    {

        ViewBag.ErrorMessage = null;

        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return BadRequest(new { errors });
        }

        var projectExists = await _projectService.ProjectExists(model.Id);
        if (!projectExists)
        {
            return NotFound(new { error = $"Project with id '{model.Id}' was not found." });
        }

        var updateProjectFormData = model.MapTo<UpdateProjectFormData>();
        var result = await _projectService.UpdateProjectAsync(updateProjectFormData);

        return Json(new { });
    }


    [HttpDelete]
    [Route("admin/projects/{id}")]
    public async Task<IActionResult> Delete(string id)
    {

        ViewBag.ErrorMessage = null;
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return BadRequest(new { errors });
        }




        var projectExists = await _projectService.ProjectExists(id);
        if (!projectExists)
        {
            return BadRequest("Finns inget projekt att ta bort");
        }

        var result = await  _projectService.DeleteProjectByIdAsync(id);

          return result.StatusCode switch
            {
                200 => Ok(),
                400 => BadRequest(result.Error),
                409 => Conflict(),
                _ => Problem(),
            };
    }





    #region Helper methods
    private async Task<IEnumerable<SelectListItem>> GetClientsSelectListAsync()
    {
        var result = await _clientService.GetClientsAsync();
        var statusList = result.Result?.Select(s => new SelectListItem
        {
            Value = s.Id,
            Text = s.ClientName,
        });

        return statusList!;
    }

    private async Task<IEnumerable<SelectListItem>> GetUsersSelectListAsync()
    {
        var result = await _userService.GetUsersAsync();
        var userResult = result.Result?.Select(u => new SelectListItem
        {
            Value = u.Id,
            Text = $"{u.FirstName} {u.LastName}"
        });
        return userResult!;
    }

    private async Task<IEnumerable<SelectListItem>> GetStatusesSelectListAsync()
    {
        var result = await _statusService.GetStatusesAsync();
        var statusList = result.Result?.Select(s => new SelectListItem
        {
            Value = s.Id.ToString(),
            Text = s.StatusName
        });

        return statusList!;
    }


    [HttpGet]
    public async Task<JsonResult> SearchUsers(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return Json(new List<object>());

        var userResult = await _userService.GetUsersAsync();
        var users = userResult.Result!;


        if (userResult.Succeeded)
        {

            var searchedUsers = users.
                Where(x => x.FirstName!.Contains(term) || x.LastName!.Contains(term) || x.Email!.Contains(term))
                .Select(x => new { x.Id, x.Image, FullName = $"{x.FirstName} {x.LastName}" });

            return Json(searchedUsers);

        }



        return Json("[]");
    }

    #endregion
}
