using System;

namespace HelpfulUtilities
{
    /// <summary>
    /// Represents a random number generator that extends upon <see cref="Random"/>
    /// </summary>
    public class AdvancedRandom : Random
    {
        /// <summary>
        /// Creates an instance of <see cref="AdvancedRandom"/>
        /// </summary>
        public AdvancedRandom() : base() { }

        /// <summary>
        /// Creates an instance of <see cref="AdvancedRandom"/>
        /// </summary>
        /// <param name="seed">The seed that should be used in generating random numbers</param>
        public AdvancedRandom(int seed) : base(seed) { }

        /// <summary>
        /// Generates a random <see cref="bool"/>
        /// </summary>
        /// <returns>The random boolean</returns>
        public bool NextBool()
        {
            var bytes = new byte[sizeof(bool)];
            NextBytes(bytes);
            return BitConverter.ToBoolean(bytes, 0);
        }

        /// <summary>
        /// Generates a byte array of the specified size filled with random bytes.
        /// </summary>
        /// <param name="size">The size of the byte array</param>
        /// <returns>The randomly filled byte array</returns>
        public byte[] NextBytes(int size)
        {
            var bytes = new byte[size];
            NextBytes(bytes);
            return bytes;
        }

        #region Byte

        /// <summary>Generates a random <see cref="sbyte"/></summary>
        /// <param name="min">The minimum bound</param>
        /// <param name="max">The maximum bound</param>
        /// <returns>The random signed byte</returns>
        public sbyte Next(sbyte min = 0, sbyte max = 1)
        {
            ErrorTests.MaxMinCheck(max, min, nameof(min));
            return (sbyte)Next((byte)min, (byte)max);
        }

        /// <summary>Generates a random <see cref="byte"/></summary>
        /// <param name="min">The minimum bound</param>
        /// <param name="max">The maximum bound</param>
        /// <returns>The random byte</returns>
        public byte Next(byte min = 0, byte max = 1)
        {
            ErrorTests.MaxMinCheck(max, min);
            return (byte)(NextBytes(sizeof(byte))[0]
                % (max - min + 1) + min);
        }

        #endregion

        #region Short

        /// <summary>Generates a random <see cref="short"/></summary>
        /// <param name="min">The minimum bound</param>
        /// <param name="max">The maximum bound</param>
        /// <returns>The random short</returns>
        public short Next(short min = 0, short max = 1)
        {
            ErrorTests.MaxMinCheck(max, min, nameof(min));
            return (short)Next((ushort)min, (ushort)max);
        }

        /// <summary>Generates a random <see cref="ushort"/></summary>
        /// <param name="min">The minimum bound</param>
        /// <param name="max">The maximum bound</param>
        /// <returns>The random ushort</returns>
        public ushort Next(ushort min = 0, ushort max = 1)
        {
            ErrorTests.MaxMinCheck(max, min);
            return (ushort)(BitConverter.ToUInt16(NextBytes(sizeof(ushort)), 0)
                % (max - min + 1) + min);
        }

        #endregion

        #region Integer

        /// <summary>Generates a random <see cref="uint"/></summary>
        /// <param name="min">The minimum bound</param>
        /// <param name="max">The maximum bound</param>
        /// <returns>The random uint</returns>
        public uint Next(uint min = 0, uint max = 1)
        {
            ErrorTests.MaxMinCheck(max, min);
            return BitConverter.ToUInt32(NextBytes(sizeof(uint)), 0)
                % (max - min + 1) + min;
        }

        #endregion

        #region Long

        /// <summary>Generates a random <see cref="long"/></summary>
        /// <param name="min">The minimum bound</param>
        /// <param name="max">The maximum bound</param>
        /// <returns>The random long</returns>
        public long Next(long min = 0, long max = 1)
        {
            ErrorTests.MaxMinCheck(max, min, nameof(min));
            return (long)Next((ulong)min, (ulong)max);
        }

        /// <summary>Generates a random <see cref="ulong"/></summary>
        /// <param name="min">The minimum bound</param>
        /// <param name="max">The maximum bound</param>
        /// <returns>The random ulong</returns>
        public ulong Next(ulong min = 0, ulong max = 1)
        {
            ErrorTests.MaxMinCheck(min, max, nameof(min));
            return BitConverter.ToUInt64(NextBytes(sizeof(ulong)), 0)
                % (max - min + 1) + min;
        }

        #endregion

        #region Floating-Point

        /// <summary>Generates a random <see cref="decimal"/></summary>
        /// <param name="min">The minimum bound</param>
        /// <param name="max">The maximum bound</param>
        /// <returns>The random decimal</returns>
        public decimal Next(decimal min = 0, decimal max = 1)
        {
            int GenInt() => BitConverter.ToInt32(NextBytes(sizeof(int)), 0);
            return new decimal(GenInt(), GenInt(), GenInt(), NextBool(), Next((byte)0, 28));
        }

        /// <summary>Generates a random <see cref="double"/></summary>
        /// <param name="min">The minimum bound</param>
        /// <param name="max">The maximum bound</param>
        /// <returns>The random double</returns>
        public double Next(double min = 0, double max = 1)
        {
            ErrorTests.MaxMinCheck(max, min, nameof(min));
            return (NextDouble() * (max - min)) + min;
        }

        /// <summary>Generates a random <see cref="float"/></summary>
        /// <param name="min">The minimum bound</param>
        /// <param name="max">The maximum bound</param>
        /// <returns>The random float</returns>
        public float Next(float min = 0, float max = 1)
        {
            ErrorTests.MaxMinCheck(max, min, nameof(min));
            return (float)Next((double)min, max);
        }

        #endregion
    }
}
