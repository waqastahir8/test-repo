using AmeriCorps.Users.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmeriCorps.Users.Data.Core.Model;

//[Table("push_notification", Schema = "users")]
public sealed class PushNotification : Entity
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    [Required]
    public string Message { get; set; }

    [Required]
    public NotificationType NotificationType { get; set; }

    [Required]
    public NotificationCategory NotificationCategory { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "Sent";

    [Required]
    public bool IsRead { get; set; } = false;

    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public int UserId { get; set; }
}



//using AmeriCorps.Users.Data.Core.Enums;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace AmeriCorps.Users.Data.Core.Model;

////[Table("push_notification", Schema = "users")]
//public sealed class PushNotification:Entity
//{
//    //[Key]
//    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//    //[Column("id")]
//    //public int Id { get; set; }

//    [Required]
//    [MaxLength(255)]
//    public string Title { get; set; }

//    [Required]
//    [Column(TypeName = "text")]
//    public string Message { get; set; }

//    [Required]
//    [Column(TypeName = "text")]
//    public NotificationType NotificationType { get; set; } 

//    [Required]
//    [Column(TypeName = "character varying(20)")]
//    public NotificationCategory NotificationCategory { get; set; } 

//    [MaxLength(20)]
//    [Column(TypeName = "character varying(20)")]
//    public string Status { get; set; } = "Sent"; 

//    [Required]
//    [Column(TypeName = "boolean")]
//    public bool IsRead { get; set; } = false;

//    [Required]
//    [Column(TypeName = "timestamp with time zone")]
//    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

//    public int UserId { get; set; }
//}

