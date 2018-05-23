using System.Threading;

namespace HelpfulUtilities.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        /// Cancels the timer from running anymore
        /// </summary>
        /// <remarks>
        /// Alias for <see cref="Timer.Change(int, int)"/> with <see cref="Timeout.Infinite"/>
        /// </remarks>
        public static void Cancel(this Timer timer)
            => timer.Change(Timeout.Infinite, Timeout.Infinite);
    }
}
