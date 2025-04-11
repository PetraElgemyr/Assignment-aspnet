using Domain.Models;

namespace Presentation.Models;

public class MembersViewModel
{
    public IEnumerable<User> Members { get; set; } = [];
    public AddUserViewModel AddUserViewModel { get; set; } = new();
}
