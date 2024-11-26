namespace AmeriCorps.Users.Models;

public class CountryOfBirthResponse : CountryOfBirthRequestModel{
   public int Id { get; set; }
    public int UserId { get; set; }
}