namespace AmeriCorps.Users.Api;

public static class StringExtensions
{
	public static string Sanitize(this string value) => value.Trim().ToLowerInvariant();
}
