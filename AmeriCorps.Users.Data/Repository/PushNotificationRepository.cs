using AmeriCorps.Data;
using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data.Core.Model;
using AmeriCorps.Users.Data.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeriCorps.Users.Data.Repository;

public sealed partial class PushNotificationRepository(
    ILogger<PushNotificationRepository> logger,
    IContextFactory contextFactory,
    IOptions<UserContextOptions> options
) : RepositoryBase<RepositoryContext, UserContextOptions>(logger, contextFactory, options),
    IPushNotificationRepository
{ 

    public async Task AddAsync(PushNotification pushNotification)
    {
        try
        {
            await ExecuteAsync(async context =>
            {
                await context.PushNotification.AddAsync(pushNotification);
                await context.SaveChangesAsync();
            });
        }
        catch (Exception ex)
        {

            throw ex;
        }
       
    }

    public async Task<PushNotification> GetByNotificationIdAsync(int id)
    {
        return await ExecuteAsync(async context =>
        {
            var notification = await context.PushNotification.FindAsync(id);
            if (notification == null)
            {
                throw new KeyNotFoundException($"Notification with ID {id} not found.");
            }

            return notification;
        });
    }

    public async Task<List<PushNotification>> GetByUserIdAsync(int userId)
    {
        return await ExecuteAsync(async context =>
        {
            var notifications = await context.PushNotification
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if (notifications == null || !notifications.Any())
            {
                throw new KeyNotFoundException($"No notifications found for User ID {userId}.");
            }

            return notifications;
        });
    }

    public async Task MarkReadNotificationAsync(int notificationId)
    {
        await ExecuteAsync(async context =>
        {
            var notification = await context.PushNotification.FindAsync(notificationId);
            if (notification == null)
            {
                throw new KeyNotFoundException($"Notification with ID {notificationId} not found.");
            }

            if (!notification.IsRead)
            {
                notification.IsRead = true;
                context.PushNotification.Update(notification);
                await context.SaveChangesAsync();
            }
        });
    }

}

