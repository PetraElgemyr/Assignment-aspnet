using Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Models;

public class ProjectsViewModel
{

    public AddProjectViewModel AddProjectViewModel { get; set; } = new();
    public UpdateProjectViewModel UpdateProjectViewModel { get; set; } = new();
    public IEnumerable<ProjectViewModel> Projects { get; set; } = [];
    public IEnumerable<SelectListItem> Clients { get; set; } = [];
    public IEnumerable<SelectListItem> Users { get; set; } = [];



}
