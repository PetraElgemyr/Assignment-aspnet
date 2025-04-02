using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class AddClientViewModel
{
    [Required(ErrorMessage = "is required.")]
    [DataType(DataType.Text)]
    [Display(Name = "Client Name", Prompt = "Enter client name")]
    public string ClientName { get; set; } = null!;

    [DataType(DataType.Text)]
    public string? Image { get; set; }
}
