using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data.Core.Model;

namespace AmeriCorps.Users.Api.Services;

public interface IUserHelperService
{
    Task<bool> SendUserInviteAsync(User toInvite);

    Task<bool> ResendUserInviteAsync(User toInvite);

    Task<bool> ResendAllUserInvitesAsync();

    Task<bool> SendOperatingSiteInviteAsync(OperatingSite toInvite);

    Task<bool> SendSSAFailureEmailAsync(List<User>? userList);
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
            EmailModel email = await FormatUserInviteEmailAsync(toInvite);

            try
            {
                await _apiService.SendUserInviteEmailAsync(email);
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
            _logger.LogInformation("Unable to send invite email, null invitee info.");
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
            EmailModel email = await FormatUserInviteEmailAsync(toInvite);

            try
            {
                await _apiService.SendUserInviteEmailAsync(email);
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
            userList = await _repository.FetchInvitedUsersForReminderAsync();
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

        if (errorCount > 0)
        {
            _logger.LogInformation("Reminder email unsuccessful for {Identifier} number of users.", errorCount.ToString().Replace(Environment.NewLine, ""));
            return false;
        }
        else
        {
            return true;
        }
    }

    public async Task<bool> SendOperatingSiteInviteAsync(OperatingSite toInvite)
    {
        if (toInvite != null && !string.IsNullOrEmpty(toInvite.EmailAddress))
        {
            EmailModel email = await FormatOperatingSiteInviteEmailAsync(toInvite);

            try
            {
                await _apiService.SendOperatingSiteInviteEmailAsync(email);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to send invite email for Operating Site {Identifier}.", toInvite.OperatingSiteName.ToString().Replace(Environment.NewLine, ""));
                return false;
            }

            _logger.LogInformation("Invite email sent for {Identifier}.", toInvite.OperatingSiteName.ToString().Replace(Environment.NewLine, ""));
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> SendSSAFailureEmailAsync(List<User>? userList)
    {

        if(userList == null || userList.Count < 1)
        {
            try
            {
                userList = await _repository.FetchFailedSSAChecksAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error fetching users for invite reminder.");
                return false;
            }
        }


        var errorCount = 0;

        if (userList != null && userList.Count > 0)
        {
            for (int i = 0; i < userList.Count; i++)
            {
                var success = await SendFailedSsaNotificationAsync(userList[i]);

                if (!success)
                {
                    errorCount++;
                }
            }
        }
        else
        {
            _logger.LogInformation("No users found for email notification.");
        }

        if (errorCount > 0)
        {
            _logger.LogInformation("Notification email unsuccessful for {Identifier} number of users.", errorCount.ToString().Replace(Environment.NewLine, ""));
            return false;
        }
        else
        {
            return true;
        }
    }

    private async Task<bool> SendFailedSsaNotificationAsync(User toNotify)
    {

        if (toNotify.Id.ToString() != null && (toNotify.CommunicationMethods != null && toNotify.CommunicationMethods.Count > 0))
        {
            EmailModel email = await FormatSSAEmailAsync(toNotify);

            try
            {
                await _apiService.SendNotificationEmailAsync(email);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to send notification email for {Identifier}.", toNotify.UserName.ToString().Replace(Environment.NewLine, ""));
                return false;
            }

            _logger.LogInformation("notification email sent for {Identifier}.", toNotify.UserName.ToString().Replace(Environment.NewLine, ""));
            return true;
        }
        else
        {
            return false;
        }
    }

    private async Task<EmailModel> FormatUserInviteEmailAsync(User toInvite)
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
        if (!string.IsNullOrEmpty(toInvite.FirstName) && !string.IsNullOrEmpty(toInvite.LastName))
        {
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
        if (!string.IsNullOrEmpty(htmlTemplate))
        {
            htmlContent = string.Format(htmlTemplate, inviteeFullName, inviter, link);

            email.Recipients = recipients;
            email.Subject = subject;
            email.Content = htmlContent;
        }

        return email;
    }

    private async Task<EmailModel> FormatOperatingSiteInviteEmailAsync(OperatingSite toInvite)
    {
        EmailModel email = new EmailModel();

        string htmlTemplate = _templates.InviteUserTemplate();

        List<string> recipients = new List<string>();

        if (!string.IsNullOrEmpty(toInvite.EmailAddress))
        {
            recipients.Add(toInvite.EmailAddress);
        }

        string subject = "You're Invited";
        string opSiteName = !string.IsNullOrEmpty(toInvite.OperatingSiteName) ? toInvite.OperatingSiteName : "";
        string inviter = "";

        User? inviterUser = new User();

        if (!string.IsNullOrEmpty(toInvite.InviteUserId.ToString()))
        {
            try
            {
                inviterUser = await _repository.GetAsync(toInvite.InviteUserId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to find invitee for {Identifier}.", toInvite.OperatingSiteName.ToString().Replace(Environment.NewLine, ""));
            }
        }

        if (inviterUser != null && !string.IsNullOrEmpty(inviterUser.FirstName) && !string.IsNullOrEmpty(inviterUser.LastName))
        {
            inviter = "by " + inviterUser.FirstName + ' ' + inviterUser.LastName;
        }

        string link = "";
        string htmlContent = "";
        if (!string.IsNullOrEmpty(htmlTemplate))
        {
            htmlContent = string.Format(htmlTemplate, opSiteName, inviter, link);

            email.Recipients = recipients;
            email.Subject = subject;
            email.Content = htmlContent;
        }
        return email;
    }

    private async Task<EmailModel> FormatSSAEmailAsync(User toNotify)
    {
        EmailModel email = new EmailModel();

        string htmlTemplate = _templates.InviteUserTemplate();

        List<string> recipients = new List<string>();

        if (toNotify.CommunicationMethods != null)
        {
            for (int i = 0; i < toNotify.CommunicationMethods.Count; i++)
            {
                if (toNotify.CommunicationMethods[i].Type == "email" && toNotify.CommunicationMethods[i].IsPreferred)
                {
                    recipients.Add(toNotify.CommunicationMethods[i].Value);
                }
            }
        }

        string subject = "Verification Failed";
        string fullName = "";
        if (!string.IsNullOrEmpty(toNotify.FirstName) && !string.IsNullOrEmpty(toNotify.LastName))
        {
            fullName = toNotify.FirstName + ' ' + toNotify.LastName;
        }

        List<User>? notifyList = new List<User>();

        try
        {
            notifyList = await FetchOtherRecipients(toNotify);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to find other users for notification for {Identifier}.", toNotify.UserName.ToString().Replace(Environment.NewLine, ""));
        }

        if (notifyList != null && notifyList.Count > 0)
        {
            for (int i = 0; i < notifyList.Count; i++)
            {
                if (notifyList[i].CommunicationMethods != null)
                {
                    for (int j = 0; j < notifyList[i].CommunicationMethods.Count; j++)
                    {
                        if (notifyList[i].CommunicationMethods[j].Type == "email" && notifyList[i].CommunicationMethods[j].IsPreferred)
                        {
                            recipients.Add(notifyList[i].CommunicationMethods[j].Value);
                        }
                    }
                }
            }

        }

        string link = "";
        string htmlContent = "";
        if (!string.IsNullOrEmpty(htmlTemplate) && recipients.Count > 0)
        {
            htmlContent = string.Format(htmlTemplate, fullName, link,link);

            email.Recipients = recipients;
            email.Subject = subject;
            email.Content = htmlContent;
        }

        return email;
    }

    private async Task<List<User>> FetchOtherRecipients(User toNotify)
    {
        List<User> userList = [];

        try
        {
            if (toNotify.UserProjects != null && toNotify.UserProjects.Count > 0 
                && toNotify.UserProjects.Find(p => p.ProjectType == "VISTA") != null)
            {
                userList = await _repository.FetchVistaRecipientsAsync();
            }
            else if (toNotify.UserProjects != null && toNotify.UserProjects.Count > 0 
                && toNotify.UserProjects.Find(p => p.ProjectType == "ASN") != null)
            {
                userList = await _repository.FetchAsnRecipientsAsync();
            }
            else if (toNotify.UserProjects != null && toNotify.UserProjects.Count > 0 
                && toNotify.UserProjects.Find(p => p.ProjectType == "NCCC") != null)
            {
                userList = await _repository.FetchNcccRecipientsAsync();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching additional users for failure notification.");
            return userList;
        }

        return userList;
    }
}