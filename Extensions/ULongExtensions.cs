namespace HelpfulUtilities.Extensions
{
    public static class ULongExtensions
    {
        public static long SafeCast(this ulong value)
            => value > long.MaxValue ? long.MaxValue : (long)value;
    }
}
