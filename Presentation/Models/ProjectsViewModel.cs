using Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Models;

public class ProjectsViewModel
{

    public AddProjectViewModel AddProjectViewModel { get; set; } = new();
    public EditProjectViewModel EditProjectViewModel { get; set; } = new();
    public IEnumerable<ProjectViewModel> Projects { get; set; } = [];
    //public IEnumerable<Project> Projects { get; set; } = [];
    public IEnumerable<SelectListItem> Clients { get; set; } = [];
    public IEnumerable<Status> Statuses { get; set; } = [];
    public IEnumerable<SelectListItem> Users { get; set; } = [];



}
