using Domain.Models;

namespace Presentation.Models;

public class ProjectViewModel
{
    public string Id { get; set; } = null!;
    public string? Image { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime Created { get; set; }
    public decimal? Budget { get; set; }
    public string ClientId { get; set; } = null!;
    public Client Client { get; set; } = null!;
    public int StatusId { get; set; }
    public Status Status { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
    
}
