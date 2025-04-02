using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Presentation.Controllers;

[Authorize]
public class ProjectsController(IProjectService projectService) : Controller
{

    private readonly IProjectService _projectService = projectService;

    [Route("admin/projects")]
    public async Task<IActionResult> Index()
    {

        //var model = new ProjectsViewModel
        //{
        //    Projects = await _projectService.GetProjectsAsync()
        //};
        //return View(model);

        return View();
    }


    //[HttpPost]
    //public async Task<IActionResult> Add(AddProjectViewModel model)
    //{
    //    var addProjectFormData = model.MapTo<AddProjectFormData>();
    //    var result = await _projectService.CreateProjectAsync(addProjectFormData);

    //    return Json(new { });
    //}


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
