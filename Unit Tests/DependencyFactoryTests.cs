using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpfulUtilities.Discord.Listeners;
using System;
using Discord.Commands;
using Discord;
using System.Reflection;
using System.Linq;

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
            var obj = DependencyManager.Factory
                .SetDependencies(new DependencyTest("Hi, my name is Fred."))
                .WithType<UnsettableTestClass>()
                .Build()
                .CreateObject<UnsettableTestClass>();
            obj.GetType().GetProperty(nameof(obj.Base)).SetValue(obj, new DependencyTest("Hello, I am Jerry."));
            var o = obj.Base;
            Assert.AreEqual("Hello, I am Jerry.", obj.Base.MyString);
        }

        [TestMethod]
        public void ModuleCreation()
        {
            try
            {
                var context = new MyContext();
                var manager = DependencyManager.Factory
                    .SetDependencies(context)
                    .WithType<AnotherModule>()
                    .Build();
                Assert.IsNotNull(manager);
                var obj = manager.CreateObject<AnotherModule>();
                Assert.IsNotNull(obj);

                if (obj is ModuleBase module)
                {
                    manager.InjectPrivateProperty(obj, context, nameof(module.Context), typeof(ModuleBase<ICommandContext>));
                    Assert.AreEqual(context, obj.Context);
                }
                else
                {
                    Assert.Fail();
                }
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void PropertiesTest()
        {
            try
            {
                var o = new Bar();
                foreach (var f in typeof(Foo).GetFields())
                {
                    Console.WriteLine(f.Name);
                }

                var field = typeof(Foo).GetField("<Number>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
                Assert.IsNotNull(field);
                field.SetValue(o, 5);
                _ = o.Number == 5;
                Assert.AreEqual(5, o.Number);
            }
            catch// (Exception e)
            {
                Assert.Fail();
            }
        }
    }

    internal class UnsettableTestClass
    {
        public DependencyTest Base { get; private set; } = new DependencyTest("Hi, my name is Bob.");
    }

    internal class DependencyTest
    {
        public string MyString { get; }
        public DependencyTest(string value) => MyString = value;
    }

    internal class AnotherModule : ModuleBase { }

    internal class MyModule : ModuleBase<MyContext>
    {
        //public new MyContext Context { get; private set; }
    }

    internal class MyContext : ICommandContext
    {
        public IDiscordClient Client => null;

        public IGuild Guild => null;

        public IMessageChannel Channel => null;

        public IUser User => null;

        public IUserMessage Message => null;
    }

    public abstract class Foo
    {
        public int Number { get; private set; }
        public Foo() { Number = 10; }
    }

    public class Bar : Foo { }
}
