using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Models;

public class UserListRequestModel
{
    public string OrgName {get; set; } = string.Empty;

    public List<UserRequestModel> users { get; set; } = new List<UserRequestModel>();

}