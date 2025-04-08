namespace Business.Models;

public class UpdateClientFormData
{
    public string Id { get; set; } = null!;
    public string? Image { get; set; }
    public string ClientName { get; set; } = null!;
    public string? Location { get; set; }

}

