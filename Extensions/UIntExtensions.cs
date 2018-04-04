namespace HelpfulUtilities.Extensions
{
    public static class UIntExtensions
    {
        public static int SafeCast(this uint value)
            => value > int.MaxValue ? int.MaxValue : (int)value;
    }
}
