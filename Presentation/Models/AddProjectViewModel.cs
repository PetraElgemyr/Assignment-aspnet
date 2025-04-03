using Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class AddProjectViewModel
{
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
    [Display(Name = "Client", Prompt = "Enter client")]
    public string ClientId { get; set; } = null!;

    public IEnumerable<Client> Clients { get; set; } = [];


    [DataType(DataType.Text)]
    [Required(ErrorMessage = "is required")]
    [Display(Name = "User", Prompt = "Enter user")]
    public string UserId { get; set; } = null!;

    [DataType(DataType.Text)]
    [Required(ErrorMessage = "is required")]
    [Display(Name = "Status", Prompt = "Enter status")]
    public int StatusId { get; set; }

}


