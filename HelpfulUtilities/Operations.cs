using System;
using System.Threading;
using System.Threading.Tasks;

namespace HelpfulUtilities
{
    /// <summary>
    /// This class provides several useful various operations.
    /// </summary>
    public static class Operations
    {
        private static readonly AdvancedRandom RNG = new AdvancedRandom();

        /// <summary>
        /// This method randoms adds or subtracts the first from the second.
        /// </summary>
        /// <param name="first">The first number to be operated on.</param>
        /// <param name="second">The second number to be operated on.</param>
        /// <returns>The result of the operation.</returns>
        public static ulong PlusMinus(ulong first, ulong second)
            => RandomOperation(first, second, (f, s) => f + s, (f, s) => f - s);

        /// <summary>
        /// This method randomly choose and performs an operation in <paramref name="functions"/> based off the 
        /// two operands, <paramref name="first"/> and <paramref name="second"/>
        /// </summary>
        /// <typeparam name="TValue">Type of operands throughout operations</typeparam>
        /// <param name="first">The first operand to be operated on.</param>
        /// <param name="second">The second operand to be operated on.</param>
        /// <param name="functions">A sequence of functions specifying operations 
        /// for <paramref name="first"/> and <paramref name="second"/></param>
        /// <returns>The result of the operation.</returns>
        public static TValue RandomOperation<TValue>(TValue first, TValue second, params Func<TValue, TValue, TValue>[] functions)
        {
            return functions[RNG.Next(max: functions.Length)](first, second);
        }

        /// <summary>
        /// This method delays for <paramref name="delay"/> milliseconds.
        /// </summary>
        /// <param name="delay">How long to wait in milliseconds.</param>
        /// <seealso cref="Delay(ulong)"/>
        public static async Task DelayAsync(ulong delay)
        {
            var elapsed = 0ul;
            while (elapsed < delay)
            {
                var remaining = delay - elapsed;
                var next = remaining >= long.MaxValue / 10000 ? long.MaxValue / 10000 : (long)remaining;
                await Task.Delay(new TimeSpan(next * 10000));
                elapsed += (ulong)next * 10000;
            }
        }

        /// <summary>
        /// This method delays for <paramref name="delay"/> milliseconds.
        /// </summary>
        /// <param name="delay">How long to wait in milliseconds.</param>
        /// <seealso cref="DelayAsync(ulong)"/>
        public static void Delay(ulong delay)
        {
            var elapsed = 0ul;
            while (elapsed < delay)
            {
                var remaining = delay - elapsed;
                var next = remaining >= int.MaxValue ? int.MaxValue : (int)remaining;
                Thread.Sleep(next);
                elapsed += (ulong)next;
            }
        }

    }
}
