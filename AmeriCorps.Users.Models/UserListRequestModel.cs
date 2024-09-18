using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Models;

public class UserListRequestModel
{
    public string OrgCode {get; set; } = string.Empty;

    public List<UserResponse> Users { get; set; } = new List<UserResponse>();

}