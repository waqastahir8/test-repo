namespace AmeriCorps.Users.Api.Services;

public sealed class MapperUtils
{

    public static List<TDestination> MapList<TSource, TDestination>(
                    IEnumerable<TSource> sourceList,
                    Func<TSource, TDestination> mapFunction) =>
                                    sourceList.Select(mapFunction).ToList();
}