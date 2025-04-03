using Domain.Models;

namespace Presentation.Models;

public class ClientsViewModel
{
    public AddClientViewModel AddClientViewModel { get; set; } = new();
    public IEnumerable<Client> Clients { get; set; } = [];
}
