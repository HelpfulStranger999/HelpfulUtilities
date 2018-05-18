using HelpfulUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unit_Tests
{
    [TestClass]
    public class RandomTests
    {
        public AdvancedRandom Random { get; } = new AdvancedRandom(false);

        [TestMethod]
        public void TestBool()
        {

        }
    }
}
