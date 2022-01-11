using System.Reactive.Linq;
using NUnit.Framework;
using Pi.HifiControl.Comms;
using Shouldly;

namespace Pi.HifiControl.Tests;

[TestFixture]
public class MessageAggregatorTests
{
    [Test]
    public void ShouldObserveCorrectStrings()
    {
        var inputs = new[]
        {
                "hello\r",
                "there\r",
                "my\rfrie",
                "nd"
            };

        inputs.ToObservable().ObserveTerminatedStrings().ToEnumerable().ShouldBe(new[] { "hello", "there", "my", "friend" });
    }
}
