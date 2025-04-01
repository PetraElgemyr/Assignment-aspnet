using Data.Contexts;
using Data.Entitites;
using Domain.Models;

namespace Data.Repositories;

public interface IUserRepository : IBaseRepository<UserEntity, User>
{

}

public class UserRepository(AppDbContext context) : BaseRepository<UserEntity, User>(context), IUserRepository
{
}
