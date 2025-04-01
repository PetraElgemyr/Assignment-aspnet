using Data.Contexts;
using Data.Entitites;
using Domain.Models;

namespace Data.Repositories;

public interface INotificationTargetRepository : IBaseRepository<NotificationTargetEntity, NotificationTarget>
{
}

public class NotificationTargetRepository(AppDbContext context) : BaseRepository<NotificationTargetEntity, NotificationTarget>(context), INotificationTargetRepository
{
}
