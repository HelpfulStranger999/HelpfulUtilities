using System;
using System.Collections.Generic;
using System.Text;

namespace HelpfulUtilities
{
    public static class ErrorTests
    {
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
