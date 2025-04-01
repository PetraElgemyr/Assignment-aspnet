using Data.Contexts;
using Data.Entitites;
using Domain.Models;

namespace Data.Repositories;

public interface INotificationTypeRepository : IBaseRepository<NotificationTypeEntity, NotificationType>
{

}
public class NotificationTypeRepository(AppDbContext context) : BaseRepository<NotificationTypeEntity, NotificationType>(context), INotificationTypeRepository
{
}
