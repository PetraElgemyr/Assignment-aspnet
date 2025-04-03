using Domain.Models;

namespace Presentation.Models;

public class ProjectsViewModel
{

    public AddProjectViewModel AddProjectViewModel { get; set; } = new();
    public IEnumerable<Project> Projects { get; set; } = [];
    public IEnumerable<Client> Clients { get; set; } = [];
    public IEnumerable<Status> Statuses { get; set; } = [];
    public IEnumerable<User> Users { get; set; } = [];

}
