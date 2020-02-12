using NUnit.Framework;
using Pi.HifiControl.Comms;
using Shouldly;

namespace Pi.HifiControl.Tests
{
    [TestFixture]
    public class MessageTests
    {
        [Test]
        public void ShouldParseMessageWithoutData()
        {
            Message.TryParse("#01,01").ShouldNotBeNull();
        }

        [Test]
        public void ShouldParseMessageWithData()
        {
            Message.TryParse("#02,03,0").ShouldNotBeNull();
        }

        [Test]
        public void ShouldFormatMessageWithoutDataCorrectly()
        {
            new Message(1, 2).ToString().ShouldBe("#01,02");
        }

        [Test]
        public void ShouldFormatMessageWithDataCorrectly()
        {
            new Message(1, 2, "Data").ToString().ShouldBe("#01,02,Data");
        }
    }
}