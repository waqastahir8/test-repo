using AmeriCorps.Users.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmeriCorps.Users.Models;

public class PushNotificationModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "UserId is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "UserId must be positive")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Message is required.")]
    public string Message { get; set; }

    [Required(ErrorMessage = "NotificationType is required.")]
    [EnumDataType(typeof(NotificationType), ErrorMessage = "Invalid NotificationType.")]
    public NotificationType NotificationType { get; set; } 

    [Required(ErrorMessage = "NotificationCategory is required.")]
    [EnumDataType(typeof(NotificationCategory), ErrorMessage = "Invalid NotificationCategory.")]
    public NotificationCategory NotificationCategory { get; set; } 

    [Required(ErrorMessage = "IsRead status is required.")]
    [Column(TypeName = "bit")]
    public bool IsRead { get; set; } = false;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}


