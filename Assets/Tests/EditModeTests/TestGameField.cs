using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;
using FluentAssertions;

public class TestGameField
{
    private GameObject gameFieldObj;
    private GameField gameField;
    private GameObject inputManagerObj;
    private List<Transform> dummyTransforms;

    [SetUp]
    public void Setup()
    {
        GameObject gameActionsObj = new GameObject("GameActions", typeof(GameActions));
        gameFieldObj = new GameObject("GameField", typeof(GameField));
        gameField = gameFieldObj.GetComponent<GameField>();
        inputManagerObj = new GameObject("InputManager", typeof(InputManager));

        dummyTransforms = new List<Transform>();
        for (int i = 0; i < 16; i++)
        {
            GameObject go = new GameObject("Slot" + i);
            go.transform.position = new Vector3(i, 0, 0);
            dummyTransforms.Add(go.transform);
        }

        FieldInfo field = typeof(GameField).GetField("transforms", BindingFlags.Instance | BindingFlags.NonPublic);
        field.SetValue(gameField, dummyTransforms);
        MethodInfo awakeMethod = typeof(GameField).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic);
        awakeMethod.Invoke(gameField, null);
    }

    [TearDown]
    public void Teardown()
    {
        UnityEngine.Object.DestroyImmediate(gameFieldObj);
        foreach (var t in dummyTransforms)
        {
            UnityEngine.Object.DestroyImmediate(t.gameObject);
        }
    }

    [Test]
    public void GetEmptyPosition_ReturnsAValidSlot_WhenBoardIsEmpty()
    {
        Pair<int, int> pos = gameField.GetEmptyPosition();
        pos.Should().NotBeNull();
    }

    [Test]
    public void CalculateSum_ReturnsCorrectSum()
    {
        FieldInfo cellsField = typeof(GameField).GetField("cells", BindingFlags.Instance | BindingFlags.Public);
        Cell[,] cells = new Cell[4, 4];
        cells[0, 0] = new Cell(2, Vector3.zero);
        cells[0, 1] = new Cell(4, Vector3.zero);
        cellsField.SetValue(gameField, cells);
        int sum = gameField.CalculateSum();
        sum.Should().Be(6);
    }

    [Test]
    public void GetEmptyPosition_ThrowsException_WhenBoardIsFull()
    {
        FieldInfo cellsField = typeof(GameField).GetField("cells", BindingFlags.Instance | BindingFlags.Public);
        Cell[,] cells = new Cell[4, 4];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                cells[i, j] = new Cell(2, Vector3.zero);
            }
        }
        cellsField.SetValue(gameField, cells);
        Action act = () => gameField.GetEmptyPosition();
        act.Should().Throw<ArgumentException>();
    }
}
