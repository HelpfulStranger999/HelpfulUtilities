using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpfulUtilities.Discord.Listeners;

namespace Unit_Tests
{
    [TestClass]
    public class DependencyFactoryTests
    {
        [TestMethod]
        public void BaseFactoryCreation()
        {
            Assert.IsNotNull(DependencyManager.Factory.Build());
        }

        [TestMethod]
        public void DependencyFactoryCreation()
        {
            var obj = DependencyManager.Factory;
            obj.WithDependencies("Hello, my name is Fred");
            obj.Build();
            Assert.IsNotNull(obj);
        }
    }
}
