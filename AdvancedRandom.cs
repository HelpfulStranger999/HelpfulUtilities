using HelpfulUtilities.Extensions;
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace HelpfulUtilities
{
    public class AdvancedRandom : Random
    {
        public static readonly Random generator = new Random(DateTimeOffset.UtcNow.ToUnixTimeSeconds().SafeCast() | Environment.TickCount);

        protected bool IsSecure = false;
        protected Random RNG;
        protected RandomNumberGenerator SecureRNG;

        public AdvancedRandom(bool secure = false) : this(secure, Environment.TickCount) { }
        public AdvancedRandom(bool secure, int seed)
        {
            IsSecure = secure;
            if (secure)
            {
                SecureRNG = RandomNumberGenerator.Create();
                RNG = null;
            }
            else
            {
                SecureRNG = null;
                RNG = new Random(seed);
            }
        }

        public new bool Next() => BitConverter.ToBoolean(NextInternal(0, 1), 0);

        public double Next(double min = 0, double max = 1) => BitConverter.ToDouble(NextInternal(min, max), 0) % (max - min + 1) + min;
        public float Next(float min = 0, float max = 1) => BitConverter.ToSingle(NextInternal(min, max), 0) % (max - min + 1) + min;

        public long Next(long min = 0, long max = 1) => BitConverter.ToInt64(NextInternal(min, max), 0) % (max - min + 1) + min;
        public uint Next(uint min = 0, uint max = 1) => BitConverter.ToUInt32(NextInternal(min, max), 0) % (max - min + 1) + min;
        public ulong Next(ulong min = 0, ulong max = 1) => BitConverter.ToUInt64(NextInternal(min, max), 0) % (min - max);

        private byte[] NextInternal<T>(T min, T max)
            where T : IComparable, IFormattable, IConvertible
        {
            Test(min, max);
            var bytes = new byte[Marshal.SizeOf(typeof(T))];

            if (IsSecure)
                SecureRNG.GetBytes(bytes);
            else
                RNG.NextBytes(bytes);

            return bytes;
        }

        private void Test<T>(T min, T max) where T : IComparable
        {
            if (min.CompareTo(max) == 1)
            {
                throw new ArgumentOutOfRangeException(nameof(min),
                    "The minimum value exceeded the maximum value");
            }
        }
    }
}
