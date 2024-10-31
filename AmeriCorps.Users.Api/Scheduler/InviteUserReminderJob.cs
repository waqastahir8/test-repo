using System;
using AmeriCorps.Users.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AmeriCorps.Users.Api.Scheduler;

    public class InviteUserReminderJob(IUserHelperService service, ILogger<InviteUserReminderJob> logger)
    {
        private readonly IUserHelperService _userHelperService = service;
        private readonly ILogger<InviteUserReminderJob> _logger = logger;

        [Function("InviteUserReminderJob")]
        public async Task<IActionResult> Run([TimerTrigger("0 1 0 * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation("C# Timer trigger function executed at: {TimeNow}", DateTime.Now);

            try
            {
                await _userHelperService.ResendAllUserInvitesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to run User Invite reminder notification job");
                return new StatusCodeResult(500);
            }

            string responseMessage = "User Invite reminder notification sent successfully";

            return new OkObjectResult(responseMessage);
        }
    }
