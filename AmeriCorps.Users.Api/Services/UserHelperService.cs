using System.Data;
using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data.Core.Model;
using AmeriCorps.Users.Models;

namespace AmeriCorps.Users.Api.Services;

public interface IUserHelperService
{
    Task<bool> SendUserInviteAsync(User toInvite);

    Task<bool> ResendUserInviteAsync(User toInvite);

    Task<bool> ResendAllUserInvitesAsync();
}

public class UserHelperService : IUserHelperService
{
    private readonly ILogger _logger;

    private readonly IUserRepository _repository;

    private readonly INotificationApiClient _apiService;

    private readonly IEmailTemplatesService _templates;

    public UserHelperService(
        ILogger<UserHelperService> logger,
        IUserRepository repository,
        INotificationApiClient apiService,
        IEmailTemplatesService templates)
    {
        _logger = logger;
        _repository = repository;
        _apiService = apiService;
        _templates = templates;
    }

    public async Task<bool> SendUserInviteAsync(User toInvite)
    {
        if (toInvite != null && (toInvite.CommunicationMethods != null && toInvite.CommunicationMethods.Count > 0) && (!string.IsNullOrEmpty(toInvite.FirstName) && !string.IsNullOrEmpty(toInvite.LastName)))
        {
            EmailModel email = await FormatInviteEmail(toInvite);

            try
            {
                await _apiService.SendInviteEmailAsync(email);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to send invite email for {Identifier}.", toInvite.Id.ToString().Replace(Environment.NewLine, ""));
                return false;
            }

            _logger.LogInformation("Reminder email sent for {Identifier}.", toInvite.Id.ToString().Replace(Environment.NewLine, ""));
            return true;
        }
        else
        {
            _logger.LogError("Unable to send invite email, null invitee");
            return false;
        }
    }

    public async Task<bool> ResendUserInviteAsync(User toInvite)
    {
        var currentDate = DateTime.UtcNow;
        DateTime dateInvited = toInvite.InviteDate ?? DateTime.MaxValue;

        if (toInvite.Id.ToString() != null && (toInvite.CommunicationMethods != null && toInvite.CommunicationMethods.Count > 0) && toInvite.UserAccountStatus == UserAccountStatus.INVITED
            && dateInvited != DateTime.MinValue && DateTime.Compare(currentDate, dateInvited.AddDays(14)) > 0)
        {
            EmailModel email = await FormatInviteEmail(toInvite);

            try
            {
                await _apiService.SendInviteEmailAsync(email);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to send invite email for {Identifier}.", toInvite.UserName.ToString().Replace(Environment.NewLine, ""));
                return false;
            }

            _logger.LogInformation("Reminder email sent for {Identifier}.", toInvite.UserName.ToString().Replace(Environment.NewLine, ""));
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> ResendAllUserInvitesAsync()
    {
        List<User> userList;

        try
        {
            userList = await _repository.FetchInvitedUsersForReminder();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching users for invite reminder.");
            return false;
        }

        var errorCount = 0;

        if (userList != null && userList.Count > 0)
        {
            for (int i = 0; i < userList.Count; i++)
            {
                var success = await ResendUserInviteAsync(userList[i]);

                if (!success)
                {
                    errorCount++;
                }
            }
        } 
        else
        {
            _logger.LogInformation("No users found for invite reminder.");
        }

        if (errorCount > 0){
            _logger.LogInformation("Reminder email unsuccessful for {Identifier} number of users.", errorCount.ToString().Replace(Environment.NewLine, ""));
            return false;
        }
        else{
            return true;
        }
    }

    public async Task<EmailModel> FormatInviteEmail(User toInvite)
    {
        EmailModel email = new EmailModel();

        string htmlTemplate = _templates.InviteUserTemplate();

        List<string> recipients = new List<string>();

        if (toInvite.CommunicationMethods != null)
        {
            for (int i = 0; i < toInvite.CommunicationMethods.Count; i++)
            {
                if (toInvite.CommunicationMethods[i].Type == "email" && toInvite.CommunicationMethods[i].IsPreferred)
                {
                    recipients.Add(toInvite.CommunicationMethods[i].Value);
                }
            }
        }

        string subject = "You're Invited";
        string inviteeFullName = "";
        if(!string.IsNullOrEmpty(toInvite.FirstName) && !string.IsNullOrEmpty(toInvite.LastName)){
           inviteeFullName = toInvite.FirstName + ' ' + toInvite.LastName;
        }
        string inviter = "";

        User? inviterUser = new User();

        try
        {
            inviterUser = await _repository.GetAsync(toInvite.InviteUserId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to find invitee for {Identifier}.", toInvite.UserName.ToString().Replace(Environment.NewLine, ""));
        }

        if (inviterUser != null && !string.IsNullOrEmpty(inviterUser.FirstName) && !string.IsNullOrEmpty(inviterUser.LastName))
        {
            inviter = "by " + inviterUser.FirstName + ' ' + inviterUser.LastName;
        }

        string link = "";
        string htmlContent = "";
        if(!string.IsNullOrEmpty(htmlTemplate)){
            htmlContent = string.Format(htmlTemplate, inviteeFullName, inviter, link);

            email.Recipients = recipients;
            email.Subject = subject;
            email.Content = htmlContent;
        }
        

        return email;
    }
}