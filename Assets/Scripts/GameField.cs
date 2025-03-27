using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameField : MonoBehaviour, IData
{
    [SerializeField]
    List<Transform> transforms = new List<Transform>();


    Vector3[,] slots = new Vector3[4, 4];

    GameActions gameActions;

    public Cell[,] cells = new Cell[4, 4];

    public Transform[,] cellObjects = new Transform[4, 4];

    const float spawnCD = 0.2f;

    private void Awake()
    {
        gameActions = FindFirstObjectByType<GameActions>();
        //if (!gameActions)
        //{
        //    Debug.LogError("Game actions not found.");
        //}

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                slots[i, j] = transforms[4* i + j].position;
            }
        }
    }

    public void LoadData(GameData gameData)
    {
        bool changed = false;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (gameData.cells.cells[i, j] != null && gameData.cells.cells[i, j].Value != -1)
                {
                    cells[i, j] = gameData.cells.cells[i, j];
                    InstantVisualCell(new Pair<int, int>(i, j), cells[i, j]);
                    changed = true;
                }
            }
        }

        if (!changed)
        {
            InstantCell(2);
            InstantCell(2);
            InstantCell(2);
        }
        gameActions.UpdateScore();
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.cells.cells = cells;
    }

    public Pair<int, int> GetEmptyPosition()
    {
        List<Pair<int, int>> freeSlots = new List<Pair<int, int>>();

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (cells[i, j] == null)
                {
                    freeSlots.Add(new Pair<int, int>(i, j));
                }
            }
        }

        if (freeSlots.Count == 0)
        {
            throw new ArgumentException("NO MORE SPACES");
        }

        int id = UnityEngine.Random.Range(0, freeSlots.Count);
        return freeSlots[id];
    }

    public void CreateCell(int value)
    {
        Pair<int, int> cellPosition = GetEmptyPosition();
        Vector3 worldPosition = slots[cellPosition.Left, cellPosition.Right];
        Cell cell = new Cell(value, worldPosition);
        cells[cellPosition.Left, cellPosition.Right] = cell;

        StartCoroutine(CreateVisualCell(cellPosition, cell));
    }

    public void InstantCell(int value)
    {
        Pair<int, int> cellPosition = GetEmptyPosition();
        Vector3 worldPosition = slots[cellPosition.Left, cellPosition.Right];
        Cell cell = new Cell(value, worldPosition);
        cells[cellPosition.Left, cellPosition.Right] = cell;
        InstantVisualCell(cellPosition, cell);
    }

    void InstantVisualCell(Pair<int, int> cellPosition, Cell cell)
    {
        GameObject newCell = GameObject.Instantiate(Resources.Load<GameObject>("Cell"), cell.Position, new Quaternion());
        newCell.transform.SetParent(transform, true);
        newCell.GetComponent<CellView>().Init(cell);
        cellObjects[cellPosition.Left, cellPosition.Right] = newCell.transform;
    }

    IEnumerator CreateVisualCell(Pair<int, int> cellPosition, Cell cell)
    {
        yield return new WaitForSeconds(spawnCD);

        GameObject newCell = GameObject.Instantiate(Resources.Load<GameObject>("Cell"), cell.Position, new Quaternion());
        newCell.transform.SetParent(transform, true);
        newCell.GetComponent<CellView>().Init(cell);
        cellObjects[cellPosition.Left, cellPosition.Right] = newCell.transform;
    }

    public void MoveUp(out bool success)
    {
        success = false;
        foreach (Cell cell in cells)
        {
            if (cell != null)
            {
                cell.Merged = false;
            }
        }
        for (int i = 1; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (cells[i, j] != null)
                {
                    int new_i = i - 1;
                    bool merged = false;
                    while (new_i >= 0 && cells[new_i, j] == null)
                    {
                        new_i--;
                    }
                    if (new_i >= 0 && cells[new_i, j].Value == cells[i, j].Value && !cells[new_i, j].Merged)
                    {
                        Transform oldCell = cellObjects[new_i, j];
                        cellObjects[new_i, j] = cellObjects[i, j];
                        cellObjects[i, j] = null;
                        if (oldCell.GetComponent<CellView>().isMoving)
                        {
                            InputManager.movingCellAmount -= 1;
                        }
                        Destroy(oldCell.gameObject);
                        cells[new_i, j] = cells[i, j];
                        cells[i, j] = null;
                        cells[new_i, j].Value *= 2;
                        cells[new_i, j].Position = slots[new_i, j];
                        merged = true;
                        cells[new_i, j].Merged = true;
                        success = true;

                    }
                    if (new_i == i - 1)
                    {
                        continue;
                    }

                    if (!merged)
                    {
                        cells[new_i + 1, j] = cells[i, j];
                        cells[i, j] = null;
                        cells[new_i + 1, j].Position = slots[new_i + 1, j];
                        cellObjects[new_i + 1, j] = cellObjects[i, j];
                        cellObjects[i, j] = null;
                        success = true;
                    }
                    cells[i, j] = null;
                }
            }
        }
    }

    public void MoveDown(out bool success)
    {
        success = false;
        foreach (Cell cell in cells)
        {
            if (cell != null)
            {
                cell.Merged = false;
            }
        }
        for (int i = 2; i >= 0; i--)
        {
            for (int j = 3; j >= 0; j--)
            {
                if (cells[i, j] != null)
                {
                    int new_i = i + 1;
                    bool merged = false;
                    while (new_i < 4 && cells[new_i, j] == null)
                    {
                        new_i++;
                    }
                    if (new_i < 4 && cells[new_i, j].Value == cells[i, j].Value && !cells[new_i, j].Merged)
                    {
                        Transform oldCell = cellObjects[new_i, j];
                        cellObjects[new_i, j] = cellObjects[i, j];
                        cellObjects[i, j] = null;
                        if (oldCell.GetComponent<CellView>().isMoving)
                        {
                            InputManager.movingCellAmount -= 1;
                        }
                        Destroy(oldCell.gameObject);
                        cells[new_i, j] = cells[i, j];
                        cells[i, j] = null;
                        cells[new_i, j].Value *= 2;
                        cells[new_i, j].Position = slots[new_i, j];
                        merged = true;
                        cells[new_i, j].Merged = true;
                        success = true;

                    }
                    if (new_i == i + 1)
                    {
                        continue;
                    }

                    if (!merged)
                    {
                        cells[new_i - 1, j] = cells[i, j];
                        cells[i, j] = null;
                        cells[new_i - 1, j].Position = slots[new_i - 1, j];
                        cellObjects[new_i - 1, j] = cellObjects[i, j];
                        cellObjects[i, j] = null;
                        success = true;
                    }
                    cells[i, j] = null;
                }
            }
        }
    }

    public void MoveLeft(out bool success)
    {
        success = false;
        foreach (Cell cell in cells)
        {
            if (cell != null)
            {
                cell.Merged = false;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j < 4; j++)
            {
                if (cells[i, j] != null)
                {
                    int new_j = j - 1;
                    bool merged = false;
                    while (new_j >= 0 && cells[i, new_j] == null)
                    {
                        new_j--;
                    }
                    if (new_j >= 0 && cells[i, new_j].Value == cells[i, j].Value && !cells[i, new_j].Merged)
                    {
                        Transform oldCell = cellObjects[i, new_j];
                        cellObjects[i, new_j] = cellObjects[i, j];
                        cellObjects[i, j] = null;
                        if (oldCell.GetComponent<CellView>().isMoving)
                        {
                            InputManager.movingCellAmount -= 1;
                        }
                        Destroy(oldCell.gameObject);
                        cells[i, new_j] = cells[i, j];
                        cells[i, j] = null;
                        cells[i, new_j].Value *= 2;
                        cells[i, new_j].Position = slots[i, new_j];
                        merged = true;
                        cells[i, new_j].Merged = true;
                        success = true;

                    }
                    if (new_j == j - 1)
                    {
                        continue;
                    }

                    if (!merged)
                    {
                        cells[i, new_j + 1] = cells[i, j];
                        cells[i, j] = null;
                        cells[i, new_j + 1].Position = slots[i, new_j + 1];
                        cellObjects[i, new_j + 1] = cellObjects[i, j];
                        cellObjects[i, j] = null;
                        success = true;
                    }
                    cells[i, j] = null;
                }
            }
        }
    }

    public void MoveRight(out bool success)
    {
        success = false;
        foreach (Cell cell in cells)
        {
            if (cell != null)
            {
                cell.Merged = false;
            }
        }
        for (int i = 3; i >= 0; i--)
        {
            for (int j = 2; j >= 0; j--)
            {
                if (cells[i, j] != null)
                {
                    int new_j = j + 1;
                    bool merged = false;
                    while (new_j < 4 && cells[i, new_j] == null)
                    {
                        new_j++;
                    }
                    if (new_j < 4 && cells[i, new_j].Value == cells[i, j].Value && !cells[i, new_j].Merged)
                    {
                        Transform oldCell = cellObjects[i, new_j];
                        cellObjects[i, new_j] = cellObjects[i, j];
                        cellObjects[i, j] = null;
                        if (oldCell.GetComponent<CellView>().isMoving)
                        {
                            InputManager.movingCellAmount -= 1;
                        }
                        Destroy(oldCell.gameObject);
                        cells[i, new_j] = cells[i, j];
                        cells[i, j] = null;
                        cells[i, new_j].Value *= 2;
                        cells[i, new_j].Position = slots[i, new_j];
                        merged = true;
                        cells[i, new_j].Merged = true;
                        success = true;

                    }
                    if (new_j == j + 1)
                    {
                        continue;
                    }

                    if (!merged)
                    {
                        cells[i, new_j - 1] = cells[i, j];
                        cells[i, j] = null;
                        cells[i, new_j - 1].Position = slots[i, new_j - 1];
                        cellObjects[i, new_j - 1] = cellObjects[i, j];
                        cellObjects[i, j] = null;
                        success = true;
                    }
                    cells[i, j] = null;
                }
            }
        }
    }

    public bool CheckForPossibleMoves()
    {
        for (int i = 1; i < 4; i++)
        {
            for (int j = 1; j < 4; j++)
            {
                if (cells[i, j].Value == cells[i - 1, j].Value || cells[i, j].Value == cells[i, j - 1].Value)
                {
                    return true;
                }
            }
        }
        for (int i = 1; i < 4; i++)
        {
            if (cells[i, 0].Value == cells[i - 1, 0].Value)
            {
                return true;
            }
            if (cells[0, i].Value == cells[0, i - 1].Value)
            {
                return true;
            }
        }
        return false;
    }

    public int CalculateSum()
    {
        int result = 0;

        foreach (Cell cell in cells)
        {
            if (cell != null)
            {
                result += cell.Value;
            }
        }

        return result;
    }
}
