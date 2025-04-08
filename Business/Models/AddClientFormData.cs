using Data.Entitites;
using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class AddClientFormData
{
    public string? Image { get; set; }
    public string ClientName { get; set; } = null!;
    public string? Location { get; set; }


}

