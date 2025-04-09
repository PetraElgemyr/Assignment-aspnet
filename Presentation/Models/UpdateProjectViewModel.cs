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

    [DataType(DataType.Text)]
    public string? Image { get; set; }

    [Required(ErrorMessage = "is required.")]
    [DataType(DataType.Text)]
    [Display(Name = "Project Name", Prompt = "Enter project name")]
    public string ProjectName { get; set; } = null!;

    [DataType(DataType.Text)]
    [Display(Name = "Description", Prompt = "Enter description (optional)")]
    public string? Description { get; set; }


    [DataType(DataType.Date)]
    [Display(Name = "Start date", Prompt = "Enter start date (optional)")]
    public DateTime? StartDate { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Start date", Prompt = "Enter start date (optional)")]
    public DateTime? EndDate { get; set; }


    [Display(Name = "Budget", Prompt = "Enter budget (optional)")]
    public decimal? Budget { get; set; }



    [DataType(DataType.Text)]
    [Required(ErrorMessage = "is required")]
    [Display(Name = "Client", Prompt = "Select a client")]
    public string ClientId { get; set; } = null!;


    [DataType(DataType.Text)]
    [Required(ErrorMessage = "is required")]
    [Display(Name = "Status", Prompt = "Select a status")]
    public string StatusId { get; set; } = null!;



    [DataType(DataType.Text)]
    [Required(ErrorMessage = "is required")]
    [Display(Name = "User", Prompt = "Select a members")]
    public string UserId { get; set; } = null!;


}