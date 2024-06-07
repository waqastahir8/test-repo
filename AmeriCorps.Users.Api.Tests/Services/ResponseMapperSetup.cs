using AmeriCorps.Users.Api.Services;

namespace AmeriCorps.Users.Api.Tests;
public class ResponseMapperSetup : BaseTests<ResponseMapper>
{
    protected override ResponseMapper Setup()
    {
        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new();
    }
}