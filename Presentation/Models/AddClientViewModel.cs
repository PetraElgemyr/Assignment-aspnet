using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class AddClientViewModel
{
    [Required(ErrorMessage = "is required.")]
    [DataType(DataType.Text)]
    [Display(Name = "Client Name", Prompt = "Enter client name")]
    public string ClientName { get; set; } = null!;

    [DataType(DataType.Upload)]
    public IFormFile? Image { get; set; }

    [DataType(DataType.Text)]
    [Display(Name = "Location", Prompt = "Enter location (optional)")]
    public string? Location { get; set; }
}
