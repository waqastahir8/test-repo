namespace AmeriCorps.Users.Api {

public sealed class Address
{
    public int Id { get; set; } 
    public bool IsForeign { get; set; }
    public string Type { get; set;} = string.Empty;
    public required string Street1 { get; set; }
    public required string Street2 { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string Country { get; set; }
    public required string ZipCode { get; set; }
    public bool MovingWithinSixMonths { get; set; }
}
}