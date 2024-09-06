namespace AmeriCorps.Users.Configuration;

public class NotificationOptions : ContextOptions
{
    public Uri ApiUrl { get; set; } = new Uri("https://app-userservice-dev-001.azurewebsites.net");


}