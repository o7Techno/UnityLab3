using NUnit.Framework;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.TestTools;
using FluentAssertions;

public class TestGameActions
{
    private GameObject gameActionsObj;
    private GameActions gameActions;
    private GameObject gameFieldObj;
    private GameField gameField;
    private GameObject inputManagerObj;
    private GameObject gameoverScreen;

    private List<GameObject> slotObjects = new List<GameObject>();

    [SetUp]
    public void Setup()
    {
        gameActionsObj = new GameObject("GameActions", typeof(GameActions));
        gameActions = gameActionsObj.GetComponent<GameActions>();
        gameoverScreen = new GameObject("GameOverScreen");
        gameActions.gameoverScreen = gameoverScreen;
        gameoverScreen.SetActive(false);
        gameFieldObj = new GameObject("GameField");
        gameFieldObj.SetActive(false);
        gameField = gameFieldObj.AddComponent<GameField>();
        inputManagerObj = new GameObject("InputManager", typeof(InputManager));

        List<Transform> dummyTransforms = new List<Transform>();
        for (int i = 0; i < 16; i++)
        {
            GameObject slot = new GameObject("Slot" + i);
            slot.transform.position = new Vector3(i, 0, 0);
            dummyTransforms.Add(slot.transform);
            slotObjects.Add(slot);
        }
        FieldInfo transformsField = typeof(GameField).GetField("transforms", BindingFlags.Instance | BindingFlags.NonPublic);
        transformsField.SetValue(gameField, dummyTransforms);
        gameFieldObj.SetActive(true);

        gameField.cells[0, 0] = new Cell(0, Vector3.zero);
        gameField.cells[0, 1] = new Cell(0, Vector3.zero);
        gameField.cells[2, 2] = new Cell(0, Vector3.zero);
        gameField.cells[3, 2] = new Cell(0, Vector3.zero);

        gameField.cellObjects[0, 0] = new GameObject("Cell1", typeof(CellView)).transform;
        gameField.cellObjects[0, 1] = new GameObject("Cell2", typeof(CellView)).transform;
        gameField.cellObjects[2, 2] = new GameObject("Cell3", typeof(CellView)).transform;
        gameField.cellObjects[3, 2] = new GameObject("Cell4", typeof(CellView)).transform;

        FieldInfo gameFieldField = typeof(GameActions).GetField("gameField", BindingFlags.Instance | BindingFlags.NonPublic);
        gameFieldField.SetValue(gameActions, gameField);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(gameFieldObj);
        Object.DestroyImmediate(gameActionsObj);
        Object.DestroyImmediate(inputManagerObj);
        Object.DestroyImmediate(gameoverScreen);
        foreach (var slot in slotObjects)
        {
            Object.DestroyImmediate(slot);
        }
    }

    [Test]
    public void Up_NoEmptySlotAndNoMoves_ActivatesGameOverScreen()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                gameField.cells[i, j] = new Cell(i * 4 + j, Vector3.zero);
            }
        }
        gameActions.Up();
        gameoverScreen.activeSelf.Should().BeTrue("потому что при отсутствии пустых слотов и ходов должен активироваться экран окончания игры");
    }

    [Test]
    public void Down_NoEmptySlotAndNoMoves_ActivatesGameOverScreen()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                gameField.cells[i, j] = new Cell(i * 4 + j, Vector3.zero);
            }
        }
        gameActions.Down();
        gameoverScreen.activeSelf.Should().BeTrue("потому что при отсутствии пустых слотов и ходов должен активироваться экран окончания игры");
    }

    [Test]
    public void Left_NoEmptySlotAndNoMoves_ActivatesGameOverScreen()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                gameField.cells[i, j] = new Cell(i * 4 + j, Vector3.zero);
            }
        }
        gameActions.Left();
        gameoverScreen.activeSelf.Should().BeTrue("потому что при отсутствии пустых слотов и ходов должен активироваться экран окончания игры");
    }

    [Test]
    public void Right_NoEmptySlotAndNoMoves_ActivatesGameOverScreen()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                gameField.cells[i, j] = new Cell(i * 4 + j, Vector3.zero);
            }
        }
        gameActions.Right();
        gameoverScreen.activeSelf.Should().BeTrue("потому что при отсутствии пустых слотов и ходов должен активироваться экран окончания игры");
    }

    [UnityTest]
    public System.Collections.IEnumerator Up_SpawnCellAndUpdateScore_Test()
    {
        gameActions.Up();
        yield return new WaitForSeconds(0.3f);
        gameActions.score.Value.Should().Be(gameField.CalculateSum(), "потому что счёт должен обновляться после перемещения вверх");
    }

    [UnityTest]
    public System.Collections.IEnumerator Down_SpawnCellAndUpdateScore_Test()
    {
        gameActions.Down();
        yield return new WaitForSeconds(0.3f);
        gameActions.score.Value.Should().Be(gameField.CalculateSum(), "потому что счёт должен обновляться после перемещения вниз");
    }

    [UnityTest]
    public System.Collections.IEnumerator Left_SpawnCellAndUpdateScore_Test()
    {
        gameActions.Left();
        yield return new WaitForSeconds(0.3f);
        gameActions.score.Value.Should().Be(gameField.CalculateSum(), "потому что счёт должен обновляться после перемещения влево");
    }

    [UnityTest]
    public System.Collections.IEnumerator Right_SpawnCellAndUpdateScore_Test()
    {
        gameActions.Right();
        yield return new WaitForSeconds(0.3f);
        gameActions.score.Value.Should().Be(gameField.CalculateSum(), "потому что счёт должен обновляться после перемещения вправо");
    }
}
