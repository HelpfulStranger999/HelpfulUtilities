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
        public void EnumerableTest()
        {
            IEnumerable<int> Power(int number, int exponent)
            {
                int result = 1;

                for (int i = 0; i < exponent; i++)
                {
                    result = result * number;
                    yield return result;
                }
            }

            var numbers = Power(2, 8);
            foreach (var n in numbers)
            {
                Console.WriteLine(n);
            }
        }

        [TestMethod]
        public async void TestModuleFinder()
        {
            try
            {
                var service = new ListenerService();
                var moduleBase = Assembly.GetAssembly(typeof(CommandService)).GetType("Discord.Commands.IModuleBase", true);
                var list = new List<Type>();

                foreach (var type in Assembly.GetAssembly(typeof(ListenerModuleOne)).GetTypes())
                {
                    var result = type.Extends(moduleBase);
                    if (result)
                    {
                        list.Add(type);
                    }

                }
                var types = Assembly.GetAssembly(typeof(ListenerModuleOne)).GetTypes().Where(type => type.Extends(moduleBase)).ToList();
                await service.AddModulesAsync(Assembly.GetAssembly(typeof(ListenerModuleOne))).ToList();
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
