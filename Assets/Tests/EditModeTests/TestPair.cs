using NUnit.Framework;
using FluentAssertions;

public class TestPair
{
    [Test]
    public void Pair_HoldsCorrectValues()
    {
        Pair<int, string> pair = new Pair<int, string>(1, "test");
        pair.Left.Should().Be(1, "так как левая часть пары должна равняться 1");
        pair.Right.Should().Be("test", "так как правая часть пары должна равняться \"test\"");
    }
}