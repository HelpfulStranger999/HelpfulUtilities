using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace HelpfulUtilities
{
    /// <summary>
    /// Represents an advanced random number generator
    /// </summary>
    public class AdvancedRandom
    {
        /// <summary>
        /// Whether this generates numbers securely
        /// </summary>
        public bool IsSecure = false;

        /// <summary>
        /// The insecure random number generator to use.
        /// </summary>
        protected Random RNG;

        /// <summary>
        /// The secure random number generator to use.
        /// </summary>
        protected RandomNumberGenerator SecureRNG;

        /// <summary>
        /// Creates an instance of <see cref="AdvancedRandom"/>
        /// </summary>
        /// <param name="secure">Whether this RNG should be secure.</param>
        /// <param name="seed">The seed that should be used for the random number generator if insecure.</param>
        public AdvancedRandom(bool secure = false, int seed = -1)
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
                RNG = new Random(seed == -1 ? Environment.TickCount : seed);
            }
        }

        /// <summary>
        /// Generates a random <see cref="bool"/>
        /// </summary>
        /// <returns>The random boolean</returns>
        public bool Next() => BitConverter.ToBoolean(NextInternal(0, 1), 0);

        /// <summary>Generates a random <see cref="int"/></summary>
        /// <param name="min">The minimum bound</param><param name="max">The maximum bound</param>
        /// <returns>The random int</returns>
        public int Next(int min = 0, int max = 1) => BitConverter.ToInt32(NextInternal(min, max), 0) % (max - min + 1) + min;
        
        /// <summary>Generates a random <see cref="double"/></summary>
        /// <param name="min">The minimum bound</param> <param name="max">The maximum bound</param>
        /// <returns>The random double</returns>
        public double Next(double min = 0, double max = 1) => BitConverter.ToDouble(NextInternal(min, max), 0) % (max - min + 1) + min;

        /// <summary>Generates a random <see cref="float"/></summary>
        /// <param name="min">The minimum bound</param> <param name="max">The maximum bound</param>
        /// <returns>The random float</returns>
        public float Next(float min = 0, float max = 1) => BitConverter.ToSingle(NextInternal(min, max), 0) % (max - min + 1) + min;


        /// <summary>Generates a random <see cref="long"/></summary>
        /// <param name="min">The minimum bound</param> <param name="max">The maximum bound</param>
        /// <returns>The random long</returns>
        public long Next(long min = 0, long max = 1) => BitConverter.ToInt64(NextInternal(min, max), 0) % (max - min + 1) + min;

        /// <summary>Generates a random <see cref="uint"/></summary>
        /// <param name="min">The minimum bound</param> <param name="max">The maximum bound</param>
        /// <returns>The random uint</returns>
        public uint Next(uint min = 0, uint max = 1) => BitConverter.ToUInt32(NextInternal(min, max), 0) % (max - min + 1) + min;

        /// <summary>Generates a random <see cref="ulong"/></summary>
        /// <param name="min">The minimum bound</param> <param name="max">The maximum bound</param>
        /// <returns>The random ulong</returns>
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
