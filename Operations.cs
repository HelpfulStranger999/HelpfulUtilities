using System;
using System.Threading;
using System.Threading.Tasks;

namespace HelpfulUtilities
{
    public static class Operations
    {
        private static readonly AdvancedRandom RNG = new AdvancedRandom();

        public static ulong PlusMinus(ulong first, ulong second)
            => RandomOperation(first, second, (f, s) => f + s, (f, s) => f - s);

        public static TValue RandomOperation<TValue>(TValue first, TValue second, params Func<TValue, TValue, TValue>[] functions)
        {
            return functions[RNG.Next(max: functions.Length)](first, second);
        }

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
