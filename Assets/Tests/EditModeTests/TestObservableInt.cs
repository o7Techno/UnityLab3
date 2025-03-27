using NUnit.Framework;
using FluentAssertions;

public class TestObservableInt
{
    [Test]
    public void ValueChange_FiresOnValueChangedEvent()
    {
        ObservableInt obs = new ObservableInt();
        int received = 0;
        obs.OnValueChanged += (val) => received = val;

        obs.Value = 5;
        received.Should().Be(5, "the event should fire and update the received value");
    }
}
