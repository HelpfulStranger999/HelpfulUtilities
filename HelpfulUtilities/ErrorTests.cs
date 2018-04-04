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
    }
}
