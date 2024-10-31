namespace AmeriCorps.Users.Models;

public sealed class ServiceResponse<T>
{
    public bool Successful { get; set; }

    public T? Content { get; set; }
}