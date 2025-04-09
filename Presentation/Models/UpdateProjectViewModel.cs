using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class UpdateProjectViewModel
{

    public IEnumerable<SelectListItem> Users { get; set; } = [];
    public IEnumerable<SelectListItem> Clients { get; set; } = [];
    public IEnumerable<SelectListItem> Statuses { get; set; } = [];

    [Editable(allowEdit: false)]
    public string Id { get; set; } = null!;

    [DataType(DataType.Upload)]
    [Display(Name = "Project Image", Prompt = "Select project image")]
    public IFormFile? Image { get; set; }

    [Required(ErrorMessage = "is required.")]
    [DataType(DataType.Text)]
    [Display(Name = "Project Name", Prompt = "Enter project name")]
    public string ProjectName { get; set; } = null!;

    [DataType(DataType.Text)]
    [Display(Name = "Description", Prompt = "Enter description (optional)")]
    public string? Description { get; set; }


    [DataType(DataType.Date)]
    [Display(Name = "Start date", Prompt = "Enter start date (optional)")]
    public DateTime? StartDate { get; set; } = DateTime.Now;

    [DataType(DataType.Date)]
    [Display(Name = "Start date", Prompt = "Enter start date (optional)")]
    public DateTime? EndDate { get; set; } = DateTime.Now;


    [Display(Name = "Budget", Prompt = "Enter budget (optional)")]
    public decimal? Budget { get; set; }

    [DataType(DataType.Text)]
    [Required(ErrorMessage = "is required")]
    [Display(Name = "Client", Prompt = "Select a client")]
    public string ClientId { get; set; } = null!;

    [DataType(DataType.Text)]
    [Required(ErrorMessage = "is required")]
    [Display(Name = "Status", Prompt = "Select a status")]
    public int StatusId { get; set; } 




    [DataType(DataType.Text)]
    [Required(ErrorMessage = "is required")]
    [Display(Name = "User", Prompt = "Select a member")]
    public string UserId { get; set; } = null!;


}

