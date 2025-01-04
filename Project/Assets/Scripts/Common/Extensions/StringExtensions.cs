public static class StringExtensions
{
    public static string Fmt(this string formatString, params object[] args)
    {
        return string.Format(formatString, args);
    }
}
