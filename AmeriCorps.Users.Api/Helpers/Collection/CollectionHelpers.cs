using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api.Helpers.Collection;

public static class CollectionHelpers
{
    public static bool ContainsType(string? type) =>
        !string.IsNullOrWhiteSpace(type) && CollectionTypes.Types.Contains(type.ToUpper());
}