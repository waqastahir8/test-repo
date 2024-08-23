using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Models;



public sealed class UserListResponse : UserListRequestModel{
      public int Id { get; set; }
}