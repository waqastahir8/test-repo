namespace AmeriCorps.Users.Api.Controllers;
public class PushNotificationController : BaseApiController
{
    private readonly IPushNotificationService _service;

    public PushNotificationController(IPushNotificationService service)
    {
        _service = service;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] PushNotificationModel notification)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _service.AddAsync(notification);
        if (result.Status == ResponseStatus.Successful)
        {
            return CreatedAtAction(nameof(GetByNotificationId), new { id = result.Response?.Id }, result.Response);
        }

        return StatusCode(500, new { message = "An error occurred while creating the notification." });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByNotificationId(int id)
    {
        var result = await _service.GetByNotificationIdAsync(id);

        if (result.Status == ResponseStatus.Successful && result.Response != null)
        {
            return Ok(result.Response);
        }

        if (result.Status == ResponseStatus.MissingInformation)
        {
            return NotFound(new { message = $"Notification with ID {id} not found." });
        }

        return StatusCode(500, new { message = "An error occurred while retrieving the notification." });
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var result = await _service.GetByUserIdAsync(userId);

        if (result.Status == ResponseStatus.Successful && result.Response != null)
        {
            return Ok(result.Response);
        }

        if (result.Status == ResponseStatus.MissingInformation)
        {
            return NotFound(new { message = $"No notifications found for User ID {userId}." });
        }

        return StatusCode(500, new { message = "An error occurred while retrieving notifications." });
    }

    [HttpPatch("{notificationId}/mark-read")]
    public async Task<IActionResult> MarkReadNotification(int notificationId)
    {
        var result = await _service.MarkReadNotificationAsync(notificationId);

        if (result.Status == ResponseStatus.Successful)
        {
            return Ok(result.Response);
        }

        return StatusCode(500, new { message = "An error occurred while marking the notification as read." });
    }
}


