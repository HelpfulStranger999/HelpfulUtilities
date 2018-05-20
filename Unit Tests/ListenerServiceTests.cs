using Discord.Commands;
using HelpfulUtilities.Discord.Listeners;
using HelpfulUtilities.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Unit_Tests
{
    [TestClass]
    public class ListenerServiceTests
    {
        [TestMethod]
        public async Task TestModuleFinder()
        {
            try
            {
                var service = new ListenerService();
                await service.AddModulesAsync(Assembly.GetAssembly(typeof(ListenerModuleOne)));
                Assert.IsTrue(service.Listeners.Count > 0);
            }
            catch (Exception e)
            {
                Assert.IsNull(e);
            }
        }
    }


    internal class ListenerModuleOne : ModuleBase<MyContext>
    {

        [Listener]
        public async Task DoStuff()
        { }
    }

    internal class ListenerModuleTwo : ModuleBase<MyContext>
    {
        [Listener]
        public Task DoMoreStuff() => Task.CompletedTask;
    }
}
