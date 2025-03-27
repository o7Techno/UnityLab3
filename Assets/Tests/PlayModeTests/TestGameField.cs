using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using FluentAssertions;

public class TestGameField
{
    GameObject gameFieldObj;
    GameField gameField;
    GameObject inputManagerObj;
    private List<Transform> dummyTransforms;

    [SetUp]
    public void SetUp()
    {
        GameObject gameActionsObj = new GameObject("GameActions", typeof(GameActions));
        gameFieldObj = new GameObject("GameField");
        gameFieldObj.SetActive(false);
        gameField = gameFieldObj.AddComponent<GameField>();
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

        gameFieldObj.SetActive(true);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(gameFieldObj);
        Object.DestroyImmediate(inputManagerObj);
        foreach (var t in dummyTransforms)
        {
            Object.DestroyImmediate(t.gameObject);
        }
    }

    [Test]
    public void InstantCell_CreatesCell_Test()
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
        cells[0, 0] = null;
        cellsField.SetValue(gameField, cells);

        gameField.InstantCell(-9);

        gameField.cells[0, 0].Should().NotBeNull("потому что ячейка должна быть создана");
        gameField.cells[0, 0].Value.Should().Be(-9, "потому что значение должно соответствовать переданному при создании");
    }
}
