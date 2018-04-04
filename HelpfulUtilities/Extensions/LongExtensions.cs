namespace HelpfulUtilities.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        /// Returns the <paramref name="value"/> safely cast to an int.
        /// </summary>
        /// <param name="value">The long to be safely cast to an int.</param>
        /// <returns>Safely cast <paramref name="value"/></returns>
        public static int SafeCast(this long value)
        {
            if (value > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (value < int.MinValue)
            {
                return int.MinValue;
            }

            return (int)value;
        }
    }
}
