using NUnit.Framework;
using UnityEngine;
using FluentAssertions;

public class TestSerializable2DArray
{
    [Test]
    public void Serialization_And_Deserialization_WorkCorrectly()
    {
        Serializable2DArray array = new Serializable2DArray();
        array.cells[0, 0] = new Cell(2, Vector3.one);
        array.cells[1, 1] = new Cell(4, Vector3.zero);
        array.OnBeforeSerialize();

        array.OnAfterDeserialize();

        array.cells[0, 0].Should().NotBeNull("так как ячейка должна быть восстановлена после десериализации");
        array.cells[0, 0].Value.Should().Be(2, "так как значение ячейки должно остаться равным 2");
        array.cells[1, 1].Should().NotBeNull("так как ячейка должна быть восстановлена после десериализации");
        array.cells[1, 1].Value.Should().Be(4, "так как значение ячейки должно остаться равным 4");
    }
}
