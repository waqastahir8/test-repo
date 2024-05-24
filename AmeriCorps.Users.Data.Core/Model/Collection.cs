namespace AmeriCorps.Users.Data.Core;

public sealed class Collection : EntityWithUserId

{
    public int ListingId { get; set; }

    public string Type
    {
        get => _type.ToUpper();
        init => _type = value;
    }

    private readonly string _type = string.Empty;
}