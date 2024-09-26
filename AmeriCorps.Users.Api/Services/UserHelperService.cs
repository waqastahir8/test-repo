using System.Data;
using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Models;

namespace AmeriCorps.Users.Api.Services;


public interface IUserHelperService
{
    Task<bool> ResendUserInvite(User toInvite);

    Task ResendAllUserInvites();
}

public class UserHelperService : IUserHelperService
{
    private readonly ILogger _logger;

    private readonly IUserRepository _repository;

    private readonly IApiService _apiService;

    public UserHelperService(
        ILogger<UserHelperService> logger,
        IUserRepository repository,
        IApiService apiService)
    {
        _logger = logger;
        _repository = repository;
        _apiService = apiService;
    }

    public async Task<bool> ResendUserInvite(User toInvite)
    {
        var currentDate = DateTime.UtcNow;
        DateTime dateInvited = toInvite.InviteDate.GetValueOrDefault();

        if(toInvite.Id.ToString() != null && toInvite.AccountStatus != null && toInvite.AccountStatus.Equals("invited", StringComparison.OrdinalIgnoreCase) 
            && dateInvited != DateTime.MinValue && DateTime.Compare(currentDate, dateInvited.AddDays(14)) > 0)
        {

            EmailModel email = FormatInviteEmail(toInvite);

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
        }else
        {
            return false;
        }
    }

    public async Task ResendAllUserInvites()
    {
        List<User> userList = _repository.FetchInvitedUsersForReminder();

        var errorCount = 0;

        if(userList.Count > 0){
            for (int i = 0; i < userList.Count; i++)
            {
                var success = await ResendUserInvite(userList[i]);

                if(!success)
                {
                   errorCount++; 
                }
            }
        }
        _logger.LogInformation("Reminder email unsuccessful for {Identifier} number of users.", errorCount.ToString().Replace(Environment.NewLine, ""));
    }


    public static EmailModel FormatInviteEmail(User toInvite)
    {
        EmailModel email =  new EmailModel();

        // string htmlTemplate = _templates.ReferenceEmailTemplate();

        List<string> recipients =  new List<string>();

        if(String.IsNullOrEmpty(toInvite.UserName)) //String.IsNullOrEmpty(toInvite.Email)
        {
            recipients.Add(toInvite.UserName);
        }

        string subject = "You're Invited"; // Example reference link
        // string htmlContent = string.Format(htmlTemplate, recipientName, applicantFullName, referenceLink);


        email.Recipients = recipients;
        email.Subject = subject;
        // email.Conent = htmlContent;

        return email;
    }

}