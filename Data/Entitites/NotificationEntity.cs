using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entitites;

public class NotificationEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Message { get; set; } = null!;
    public string? Image { get; set; } = null!;
    public DateTime CreateDate { get; set; } = DateTime.Now;


    [ForeignKey(nameof(NotificationType))]
    public int NotificationTypeId { get; set; }
    public virtual NotificationTypeEntity NotificationType { get; set; } = null!;



    [ForeignKey(nameof(NotificationTarget))]
    public int NotificationTargetId { get; set; }
    public virtual NotificationTargetEntity NotificationTarget { get; set; } = null!;

    public virtual ICollection<UserDismissedNotificationEntity> DismissedNotifications { get; set; } = [];

}
