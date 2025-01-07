using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data.Core.Model;
using AmeriCorps.Users.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace AmeriCorps.Users.Api.ControllerServices;

public interface IPushNotificationService
{
    Task<(ResponseStatus Status, PushNotificationModel? Response)> AddAsync(PushNotificationModel pushNotification);
    Task<(ResponseStatus Status, List<PushNotificationModel>? Response)> GetByUserIdAsync(int userId);
    Task<(ResponseStatus Status, PushNotificationModel? Response)> GetByNotificationIdAsync(int id);
    Task<(ResponseStatus Status, bool Response)> MarkReadNotificationAsync(int notificationId);
}


public sealed class PushNotificationService : IPushNotificationService
{
    private readonly ILogger _logger;
    private readonly IResponseMapper _responseMapper;
    private readonly IRequestMapper _requestMapper;
    private readonly IPushNotificationRepository _repository;

    public PushNotificationService(
        ILogger<PushNotificationService> logger,
        IRequestMapper requestMapper,
        IResponseMapper responseMapper,
        IPushNotificationRepository repository)
    {
        _logger = logger;
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
        _repository = repository;
    }

    public async Task<(ResponseStatus Status, PushNotificationModel? Response)> AddAsync(PushNotificationModel pushNotification)
    {
        if (pushNotification == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        PushNotification notification = _requestMapper.Map(pushNotification);

        try
        {
            await _repository.AddAsync(notification);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to add push notification: {e.Message}", e);
            return (ResponseStatus.UnknownError, null);
        }

        var response = _responseMapper.Map(notification);
        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, PushNotificationModel? Response)> GetByNotificationIdAsync(int id)
    {
        PushNotification? data;

        try
        {
            data = await _repository.GetByNotificationIdAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to fetch notification with ID {id}: {e.Message}", e);
            return (ResponseStatus.UnknownError, null);
        }

        if (data == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var response = _responseMapper.Map(data);
        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, List<PushNotificationModel>? Response)> GetByUserIdAsync(int userId)
    {
        List<PushNotification>? notifications;

        try
        {
            notifications = await _repository.GetByUserIdAsync(userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to fetch notifications for user ID {userId}: {e.Message}", e);
            return (ResponseStatus.UnknownError, null);
        }

        if (notifications == null || !notifications.Any())
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var response = _responseMapper.Map(notifications);
        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, bool Response)> MarkReadNotificationAsync(int notificationId)
    {
        try
        {
            await _repository.MarkReadNotificationAsync(notificationId);
            return (ResponseStatus.Successful, true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to mark notification with ID {notificationId} as read: {e.Message}", e);
            return (ResponseStatus.UnknownError, false);
        }
    }
}


