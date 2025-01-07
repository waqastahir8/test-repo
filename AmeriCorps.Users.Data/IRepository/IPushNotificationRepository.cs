using AmeriCorps.Users.Data.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeriCorps.Users.Data.IRepository;
public interface IPushNotificationRepository
{
    Task<List<PushNotification>> GetByUserIdAsync(int userID);
    Task<PushNotification> GetByNotificationIdAsync(int id);
    Task AddAsync(PushNotification pushNotification);
    Task MarkReadNotificationAsync(int notificationId);
}

