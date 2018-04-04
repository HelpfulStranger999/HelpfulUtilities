namespace HelpfulUtilities.Extensions
{
    public static class LongExtensions
    {
        public static int SafeCast(this long value)
            => value > int.MaxValue ? int.MaxValue : (int)value;
    }
}
