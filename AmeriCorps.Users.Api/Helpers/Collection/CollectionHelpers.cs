using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api.Helpers.Collection;

public static class CollectionHelpers
{

    public static bool ContainsType(string type)
    {
        if (string.IsNullOrEmpty(type) || string.IsNullOrWhiteSpace(type))
            return false;

        return CollectionTypes.Types.Contains(type.ToUpper());
    }
}