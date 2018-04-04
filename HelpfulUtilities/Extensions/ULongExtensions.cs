namespace HelpfulUtilities.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        /// Returns the <paramref name="value"/> safely cast to an long.
        /// </summary>
        /// <param name="value">The ulong to be safely cast to an long.</param>
        /// <returns>Safely cast <paramref name="value"/></returns>
        public static long SafeCast(this ulong value)
        {
            if(value > long.MaxValue)
            {
                return long.MaxValue;
            }

            return (long)value;
        }
    }
}
