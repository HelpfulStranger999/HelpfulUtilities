namespace HelpfulUtilities.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        /// Returns the <paramref name="value"/> safely cast to an int.
        /// </summary>
        /// <param name="value">The uint to be safely cast to an int.</param>
        /// <returns>Safely cast <paramref name="value"/></returns>
        public static int SafeCast(this uint value)
        {
            if (value > int.MaxValue)
            {
                return int.MaxValue;
            }

            return (int)value;
        }
    }
}
