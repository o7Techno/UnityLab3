using NUnit.Framework;
using UnityEngine;
using FluentAssertions;

public class TestCell
{
    [Test]
    public void ValueProperty_FiresOnValueChangedEvent()
    {
        bool invoked = false;
        Cell cell = new Cell(2, Vector3.zero);
        cell.OnValueChanged += () => invoked = true;

        cell.Value = 4;

        invoked.Should().BeTrue("так как при установке нового значения должно вызываться событие OnValueChanged");
        cell.Value.Should().Be(4, "так как значение свойства должно обновиться после установки");
    }

    [Test]
    public void PositionProperty_FiresOnPositionChangedEvent()
    {
        bool invoked = false;
        Cell cell = new Cell(2, Vector3.zero);
        cell.OnPositionChanged += () => invoked = true;
        Vector3 newPos = new Vector3(1, 1, 1);

        cell.Position = newPos;

        invoked.Should().BeTrue("так как при установке нового положения должно вызываться событие OnPositionChanged");
        cell.Position.Should().Be(newPos, "так как положение свойства должно обновиться после установки");
    }
}
