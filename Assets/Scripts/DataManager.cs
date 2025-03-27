using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [Header("File config")]
    [SerializeField]
    string fileName = "";

    GameData gameData;

    List<IData> dataObjects;

    FileDataManager fileDataManager;

    private void Start()
    {
        fileDataManager = new FileDataManager(Application.persistentDataPath, fileName);
        dataObjects = new List<IData>(FindObjectsByType(typeof(MonoBehaviour), FindObjectsSortMode.None).OfType<IData>());
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
        fileDataManager.Clear();
    }

    public void LoadGame()
    {
        gameData = fileDataManager.Load();

        if (gameData == null)
        {
            NewGame();
        }

        foreach (IData data in dataObjects)
        {
            data.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (IData data in dataObjects)
        {
            data.SaveData(ref gameData);
        }

        fileDataManager.Save(gameData);
    }
}
