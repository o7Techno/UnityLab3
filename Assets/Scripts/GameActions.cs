using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameActions : MonoBehaviour
{

    [SerializeField]
    public GameObject gameoverScreen;

    [SerializeField]
    GameField gameField;

    [SerializeField]
    public DataManager dataManager;
    public ObservableInt score
    {
        get; private set;
    }

    private void Awake()
    {
        score = new ObservableInt();
    }

    public void Up()
    {
        bool success;
        gameField.MoveUp(out success);
        if (success)
        {
            Spawn();
            PostSpawn();
        }
        try
        {
            gameField.GetEmptyPosition();
        }
        catch (ArgumentException)
        {
            if (!gameField.CheckForPossibleMoves())
            {
                gameoverScreen.SetActive(true);
            }
        }
    }

    public void Down()
    {
        bool success;
        gameField.MoveDown(out success);
        if (success)
        {
            Spawn();
            PostSpawn();
        }
        try
        {
            gameField.GetEmptyPosition();
        }
        catch (ArgumentException)
        {
            if (!gameField.CheckForPossibleMoves())
            {
                gameoverScreen.SetActive(true);
            }
        }
    }

    public void Left()
    {
        bool success;
        gameField.MoveLeft(out success);
        if (success)
        {
            Spawn();
            PostSpawn();
        }
        try
        {
            gameField.GetEmptyPosition();
        }
        catch (ArgumentException)
        {
            if (!gameField.CheckForPossibleMoves())
            {
                gameoverScreen.SetActive(true);
            }
        }
    }

    public void Right()
    {
        bool success;
        gameField.MoveRight(out success);
        if (success)
        {
            Spawn();
            PostSpawn();
        }
        try
        {
            gameField.GetEmptyPosition();
        }
        catch (ArgumentException)
        {
            if (!gameField.CheckForPossibleMoves())
            {
                gameoverScreen.SetActive(true);
            }
        }
    }

    public void RestartGame()
    {
        dataManager.NewGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Spawn()
    {
        int value = 2;
        if (UnityEngine.Random.Range(0, 5) == 0)
        {
            value = 4;
        }
        gameField.CreateCell(value);
    }

    void PostSpawn()
    {
        UpdateScore();
    }

    public void UpdateScore()
    {
        score.Value = gameField.CalculateSum();
    }

}
