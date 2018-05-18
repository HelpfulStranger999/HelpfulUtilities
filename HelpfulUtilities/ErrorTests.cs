using System;

namespace HelpfulUtilities
{
    /// <summary>
    /// This class provides a set of checks that throw exceptions if they fail.
    /// </summary>
    public static class ErrorTests
    {
        /// <summary>
        /// This method tests for <paramref name="obj"/> being null.
        /// </summary>
        /// <param name="obj">Object being tested for null.</param>
        /// <param name="name">The name of the parameter for throwing <see cref="ArgumentNullException"/></param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="obj"/> is null</exception>
        public static void NullCheck(object obj, string name = null)
        {
            if (obj == null)
            {
                if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(name);
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// This method tests for <paramref name="min"/> being greater than <paramref name="max"/>
        /// </summary>
        /// <param name="max">The maximum integer</param>
        /// <param name="min">The minimum integer</param>
        /// <param name="name">The name of the parameter for throwing <see cref="ArgumentOutOfRangeException"/></param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="min"/> is greater than <paramref name="max"/></exception>
        public static void MaxMinCheck(int max, int min, string name = null)
        {
            if (min < max)
            {
                throw new ArgumentOutOfRangeException(name ?? nameof(min));
            }
        }

        /// <summary>
        /// This method tests for <paramref name="min"/> being greater than <paramref name="max"/>
        /// </summary>
        /// <param name="max">The maximum long</param>
        /// <param name="min">The minimum long</param>
        /// <param name="name">The name of the parameter for throwing <see cref="ArgumentOutOfRangeException"/></param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="min"/> is greater than <paramref name="max"/></exception>
        public static void MaxMinCheck(long max, long min, string name = null)
        {
            if (min < max)
            {
                throw new ArgumentOutOfRangeException(name ?? nameof(min));
            }
        }

        /// <summary>
        /// This method tests for <paramref name="min"/> being greater than <paramref name="max"/>
        /// </summary>
        /// <param name="max">The maximum short</param>
        /// <param name="min">The minimum short</param>
        /// <param name="name">The name of the parameter for throwing <see cref="ArgumentOutOfRangeException"/></param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="min"/> is greater than <paramref name="max"/></exception>
        public static void MaxMinCheck(short max, short min, string name = null)
        {
            if (min < max)
            {
                throw new ArgumentOutOfRangeException(name ?? nameof(min));
            }
        }

        /// <summary>
        /// This method tests for <paramref name="min"/> being greater than <paramref name="max"/>
        /// </summary>
        /// <param name="max">The maximum unsigned integer</param>
        /// <param name="min">The minimum unsigned integer</param>
        /// <param name="name">The name of the parameter for throwing <see cref="ArgumentOutOfRangeException"/></param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="min"/> is greater than <paramref name="max"/></exception>
        public static void MaxMinCheck(uint max, uint min, string name = null)
        {
            if (min < max)
            {
                throw new ArgumentOutOfRangeException(name ?? nameof(min));
            }
        }

        /// <summary>
        /// This method tests for <paramref name="min"/> being greater than <paramref name="max"/>
        /// </summary>
        /// <param name="max">The maximum unsigned long</param>
        /// <param name="min">The minimum unsigned long</param>
        /// <param name="name">The name of the parameter for throwing <see cref="ArgumentOutOfRangeException"/></param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="min"/> is greater than <paramref name="max"/></exception>
        public static void MaxMinCheck(ulong max, ulong min, string name = null)
        {
            if (min < max)
            {
                throw new ArgumentOutOfRangeException(name ?? nameof(min));
            }
        }

        /// <summary>
        /// This method tests for <paramref name="min"/> being greater than <paramref name="max"/>
        /// </summary>
        /// <param name="max">The maximum unsigned short</param>
        /// <param name="min">The minimum unsigned short</param>
        /// <param name="name">The name of the parameter for throwing <see cref="ArgumentOutOfRangeException"/></param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="min"/> is greater than <paramref name="max"/></exception>
        public static void MaxMinCheck(ushort max, ushort min, string name = null)
        {
            if (min < max)
            {
                throw new ArgumentOutOfRangeException(name ?? nameof(min));
            }
        }

        /// <summary>
        /// This method tests for <paramref name="min"/> being greater than <paramref name="max"/>
        /// </summary>
        /// <param name="max">The maximum byte</param>
        /// <param name="min">The minimum byte</param>
        /// <param name="name">The name of the parameter for throwing <see cref="ArgumentOutOfRangeException"/></param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="min"/> is greater than <paramref name="max"/></exception>
        public static void MaxMinCheck(byte max, byte min, string name = null)
        {
            if (min < max)
            {
                throw new ArgumentOutOfRangeException(name ?? nameof(min));
            }
        }

        /// <summary>
        /// This method tests for <paramref name="min"/> being greater than <paramref name="max"/>
        /// </summary>
        /// <param name="max">The maximum signed byte</param>
        /// <param name="min">The minimum signed byte</param>
        /// <param name="name">The name of the parameter for throwing <see cref="ArgumentOutOfRangeException"/></param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="min"/> is greater than <paramref name="max"/></exception>
        public static void MaxMinCheck(sbyte max, sbyte min, string name = null)
        {
            if (min < max)
            {
                throw new ArgumentOutOfRangeException(name ?? nameof(min));
            }
        }

        /// <summary>
        /// This method tests for <paramref name="min"/> being greater than <paramref name="max"/>
        /// </summary>
        /// <param name="max">The maximum floating-point value</param>
        /// <param name="min">The minimum floating-point value</param>
        /// <param name="name">The name of the parameter for throwing <see cref="ArgumentOutOfRangeException"/></param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="min"/> is greater than <paramref name="max"/></exception>
        public static void MaxMinCheck(float max, float min, string name = null)
        {
            if (min < max)
            {
                throw new ArgumentOutOfRangeException(name ?? nameof(min));
            }
        }

        /// <summary>
        /// This method tests for <paramref name="min"/> being greater than <paramref name="max"/>
        /// </summary>
        /// <param name="max">The maximum double</param>
        /// <param name="min">The minimum double</param>
        /// <param name="name">The name of the parameter for throwing <see cref="ArgumentOutOfRangeException"/></param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="min"/> is greater than <paramref name="max"/></exception>
        public static void MaxMinCheck(double max, double min, string name = null)
        {
            if (min < max)
            {
                throw new ArgumentOutOfRangeException(name ?? nameof(min));
            }
        }

        /// <summary>
        /// This method tests for <paramref name="min"/> being greater than <paramref name="max"/>
        /// </summary>
        /// <param name="max">The maximum decimal</param>
        /// <param name="min">The minimum decimal</param>
        /// <param name="name">The name of the parameter for throwing <see cref="ArgumentOutOfRangeException"/></param>
        /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="min"/> is greater than <paramref name="max"/></exception>
        public static void MaxMinCheck(decimal max, decimal min, string name = null)
        {
            if (min < max)
            {
                throw new ArgumentOutOfRangeException(name ?? nameof(min));
            }
        }
    }
}
