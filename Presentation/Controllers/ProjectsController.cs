using Business.Models;
using Business.Services;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        var clientResult = await _clientService.GetClientsAsync();
        var userResult = await _userService.GetUsersAsync();
        var statusResult = await _statusService.GetStatusesAsync();
        var model = new ProjectsViewModel();

        if (projectResult.Result!.Any() && clientResult.Result!.Any() && userResult.Result!.Any() && statusResult.Result!.Any())
        {
            model.Projects = projectResult.Result!;
            model.Clients = clientResult.Result!;
            model.Users = userResult.Result!;
            model.Statuses = statusResult.Result!;
           
            return View(model);

        }



        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Add(AddProjectViewModel model)
    {
        var addProjectFormData = model.MapTo<AddProjectFormData>();
        var result = await _projectService.CreateProjectAsync(addProjectFormData);
        return View(model);
        //return Json(new { });
    }


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
}
