using Business.Models;
using Business.Services;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Models;

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

        if (projectResult.Succeeded)
        {
            var projectViewModels = new List<ProjectViewModel>();
            var clientResult = await _clientService.GetClientsAsync();
            var clients = clientResult.Result;
            var statusResult = await _statusService.GetStatusesAsync();
            

            if (projectResult.Result!.Any())
            {
                 foreach (var project in projectResult.Result!)
                {
                    var vm = project.MapTo<ProjectViewModel>();
                    vm.Client = clients!.FirstOrDefault(x => x.Id == project.ClientId)!;
                    projectViewModels.Add(vm);
                }
            }

            var model = new ProjectsViewModel
            {
                Projects = projectViewModels,
                Clients = await GetClientsSelectListAsync(),
                //Statuses = await GetStatusesSelectListAsync(), i edit bara
                Statuses = statusResult.Result ?? [],
                AddProjectViewModel = new AddProjectViewModel(),
                EditProjectViewModel = new EditProjectViewModel(),
            };

            return View(model);
        }
        return View();
    }



    #region Add
    //[HttpGet]
    //public IActionResult Add()
    //{
    //    ViewBag.ErrorMessage = "";
    //    return View();
    //}

    [HttpPost]
    [Route("admin/projects")]
    public async Task<IActionResult> Add(AddProjectViewModel model)
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

        var addProjectFormData = model.MapTo<AddProjectFormData>();
        var result = await _projectService.CreateProjectAsync(addProjectFormData);

        return result.StatusCode switch
        {
            200 => Ok(),
            400 => BadRequest(result.Error),
            409 => Conflict(),
            _ => Problem(),
        };



    }
    #endregion


    //[HttpPut]
    //public async Task<IActionResult> Update(UpdateProjectViewModel model)
    //{

    //    var projectExists = await _projectService.ProjectExists(model.id);
    //    if (!projectExists)
    //    {
    //        return Json(new { });
    //    }

    //    var updateProjectFormData = model.MapTo<UpdateProjectFormData>();
    //    var result = await _projectService.UpdateProjectAsync(updateProjectFormData);

    //    return Json(new { });
    //}


    //[HttpDelete]
    //public async Task<IActionResult> Delete(string id)
    //{

    //    var projectExists = await _projectService.ProjectExists(id);
    //    if (!projectExists)
    //    {
    //        return Json(new { });
    //    }

    //    var result = _projectService.DeleteProjectByIdAsync(id);
    //    return Json(new { });
    //}





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
