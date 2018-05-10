using Discord;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpfulUtilities.Discord.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using HelpfulUtilities.Extensions;

namespace Unit_Tests
{
    [TestClass]
    public class EmojiTests
    {
        [TestMethod]
        public void HasEmojiExtension()
        {
            var emoji = new Emoji("😃");
            var url = emoji.GetUrl();
        }
    }
}
