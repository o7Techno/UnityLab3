using NUnit.Framework;
using UnityEngine;
using System.IO;
using FluentAssertions;

public class TestFileDataManager
{
    private string tempPath;
    private string fileName = "testGameData.json";
    private FileDataManager fileDataManager;

    [SetUp]
    public void Setup()
    {
        tempPath = Application.temporaryCachePath;
        fileDataManager = new FileDataManager(tempPath, fileName);
        fileDataManager.Clear();
    }

    [TearDown]
    public void Teardown()
    {
        fileDataManager.Clear();
    }

    [Test]
    public void SaveAndLoad_GameDataIsPersistedCorrectly()
    {
        GameData data = new GameData();
        data.cells.cells[0, 0] = new Cell(2, Vector3.one);
        fileDataManager.Save(data);

        GameData loaded = fileDataManager.Load();
        loaded.Should().NotBeNull();
        loaded.cells.cells[0, 0].Value.Should().Be(2);
        loaded.cells.cells[0, 0].Position.Should().Be(Vector3.one);
    }

    [Test]
    public void Clear_DeletesTheSavedFile()
    {
        GameData data = new GameData();
        fileDataManager.Save(data);
        string fullPath = Path.Combine(tempPath, fileName);
        File.Exists(fullPath).Should().BeTrue();
        fileDataManager.Clear();
        File.Exists(fullPath).Should().BeFalse();
    }
}
