namespace AmeriCorps.Users.Data.Core;
public class EmailModel
{

    public List<string> Recipients { get; set; } = new();

    public string Subject { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public List<string> CC { get; set; } = new();

}