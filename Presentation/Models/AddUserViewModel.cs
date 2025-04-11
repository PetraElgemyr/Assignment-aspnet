using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class AddUserViewModel
{
    [DataType(DataType.Upload)]
    public IFormFile? Image { get; set; }


    [Required(ErrorMessage = "is required.")]
    [DataType(DataType.Text)]
    [Display(Name = "First Name", Prompt = "Enter first name")]
    public string FirstName { get; set; } = null!;


    [Required(ErrorMessage = "is required.")]
    [DataType(DataType.Text)]
    [Display(Name = "Last Name", Prompt = "Enter last name")]
    public string LastName { get; set; } = null!;


    [Required(ErrorMessage = "is required.")]
    [RegularExpression(@"^[^@\s]+@[^\s]+\.[^@\s]+$", ErrorMessage = "Invalid email")]
    [Display(Name = "Email", Prompt = "Enter email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;


    [Display(Name = "Phone number", Prompt = "Enter phone number (optional)")]
    public string? PhoneNumber { get; set; } 


    [DataType(DataType.Text)]
    [Display(Name = "Job Title", Prompt = "Enter job title (optional)")]
    public string? JobTitle { get; set; }

    [Required(ErrorMessage = "is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Birth date", Prompt = "Enter birth date")]
    public DateTime DateOfBirth { get; set; } = DateTime.Now;


}
