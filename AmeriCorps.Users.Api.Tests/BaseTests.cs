namespace AmeriCorps.Users.Api.Tests;

public abstract class BaseTests<T>
{
    protected Fixture Fixture = new();

    protected abstract T Setup();
}
