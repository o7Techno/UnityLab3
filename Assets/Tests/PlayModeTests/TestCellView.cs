using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;
using System.Collections;
using System.Reflection;
using FluentAssertions;

public class TestCellView
{
    private GameObject cellViewObj;
    private CellView cellView;
    private GameObject inputManager;

    [SetUp]
    public void Setup()
    {
        inputManager = new GameObject("GameManager", typeof(GameActions));
        inputManager.AddComponent<InputManager>();
        cellViewObj = new GameObject("CellView", typeof(CellView));
        cellView = cellViewObj.GetComponent<CellView>();
        cellView.valueText = cellViewObj.AddComponent<TextMeshProUGUI>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(cellViewObj);
        Object.DestroyImmediate(inputManager);
    }

    [Test]
    public void Init_And_UpdateValue_ChangesTextAndColor()
    {
        Cell cell = new Cell(2, Vector3.zero);
        cellView.Init(cell);
        TextMeshProUGUI tmp = cellViewObj.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text.Should().Be("2", "так как изначальное значение ячейки равно 2");
        cell.Value = 4;
        tmp.text.Should().Be("4", "так как значение ячейки должно обновиться до 4");
    }

    [UnityTest]
    public IEnumerator UpdatePosition_ChangesPosition()
    {
        Cell cell = new Cell(2, Vector3.zero);
        cellView.Init(cell);
        Transform transform = cellViewObj.GetComponentInChildren<Transform>();
        transform.position.Should().Be(Vector3.zero, "так как начальное положение должно быть нулевым");
        cell.Position = Vector3.left;
        yield return new WaitForSeconds(1f);
        transform.position.Should().Be(Vector3.left, "так как после обновления позиция должна стать Vector3.left");
    }
}
