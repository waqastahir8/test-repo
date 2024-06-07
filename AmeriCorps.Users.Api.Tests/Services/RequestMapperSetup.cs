using AmeriCorps.Users.Api.Services;

namespace AmeriCorps.Users.Api.Tests;
public class RequestMapperSetup : BaseTests<RequestMapper>
{
    protected override RequestMapper Setup()
    {
        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new();
    }
}