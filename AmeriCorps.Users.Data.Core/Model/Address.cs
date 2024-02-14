namespace AmeriCorps.Users.Data.Core;

public sealed class Address
{
    public int Id { get; set; } 
    public bool IsForeign { get; set; }
    public AddressType Type { get; set;}
    public required string Street1 { get; set; }
    public required string Street2 { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string ZipCode { get; set; }
    public bool MovingWithinSixMonths { get; set; }
}

public enum AddressType {
    None, Permanent, Current
}