using System.Collections;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using FluentAssertions;

public class TestDataManager
{
    public class DummyData : MonoBehaviour, IData
    {
        public bool loadCalled = false;
        public bool saveCalled = false;
        public GameData receivedData;

        public void LoadData(GameData gameData)
        {
            loadCalled = true;
            receivedData = gameData;
        }

        public void SaveData(ref GameData gameData)
        {
            saveCalled = true;
        }
    }

    GameObject dataManagerObj;
    GameObject dummyObj;
    DataManager dataManager;
    DummyData dummy;
    string testFileName;

    [SetUp]
    public void SetUp()
    {
        dummyObj = new GameObject("DummyData", typeof(DummyData));
        dummy = dummyObj.GetComponent<DummyData>();
        dataManagerObj = new GameObject("DataManager", typeof(DataManager));
        dataManager = dataManagerObj.GetComponent<DataManager>();
        FieldInfo fileNameField = typeof(DataManager).GetField("fileName", BindingFlags.NonPublic | BindingFlags.Instance);
        testFileName = "test_gamedata.json";
        fileNameField.SetValue(dataManager, testFileName);
        dataManagerObj.SendMessage("Start");
    }

    [TearDown]
    public void TearDown()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, testFileName);
        if (File.Exists(fullPath))
            File.Delete(fullPath);

        GameObject.DestroyImmediate(dummyObj);
        GameObject.DestroyImmediate(dataManagerObj);
    }

    [UnityTest]
    public IEnumerator DataManager_SaveLoad_Test()
    {
        yield return null;
        dataManager.LoadGame();
        yield return null;
        dummy.loadCalled.Should().BeTrue("так как DummyData должен вызвать метод LoadData при загрузке игры");

        dataManager.SaveGame();
        yield return null;
        dummy.saveCalled.Should().BeTrue("так как DummyData должен вызвать метод SaveData при сохранении игры");
    }
}
