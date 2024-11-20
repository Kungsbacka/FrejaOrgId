namespace FrejaOrgId;

internal static class Extensions
{
    public static string ThrowIfNullOrEmpty(this string value, string paramName)
    {
        if (value is null) throw new ArgumentNullException(paramName);
        if (value == string.Empty) throw new ArgumentException("Value cannot be an empty string", paramName);
        return value;
    }

    public static T ThrowIfNull<T>(this T value, string paramName)
    {
        if (value is null) throw new ArgumentNullException(paramName);
        return value;
    }

    public static T[] ThrowIfNullOrTooMany<T>(this T[] value, int maxCount, string paramName)
    {
        if (value is null) throw new ArgumentNullException(paramName);
        return value.ThrowIfTooMany(maxCount, paramName)!;
    }

    public static T[]? ThrowIfTooMany<T>(this T[]? value, int maxCount, string paramName)
    {
        if (value is null) return value;
        if (value.Length > maxCount) throw new ArgumentException($"The array exceeds the maximum allowed size of {maxCount} elements.", paramName);
        return value;
    }
}
