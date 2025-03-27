using System.IO;
using System;
using UnityEngine;
using System.Diagnostics;

public class FileDataManager
{
    string path;
    string fileName;

    private readonly string codeWord = "supersecretkey";

    public FileDataManager(string path, string fileName)
    {
        this.path = path;
        this.fileName = fileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(path, fileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                dataToLoad = EncryptDecrypt(dataToLoad);

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            finally { }
            //catch (Exception ex)
            //{
            //    UnityEngine.Debug.LogError(ex.ToString());
            //}
        }
        return loadedData;
    }

    public void Save(GameData gameData)
    {
        string fullPath = Path.Combine(path, fileName);

        try
        {
            Directory.CreateDirectory(path);

            string dataToStore = JsonUtility.ToJson(gameData, true);

            dataToStore = EncryptDecrypt(dataToStore);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        finally { }
        //catch (Exception ex)
        //{
        //    UnityEngine.Debug.LogError(ex.ToString());
        //}
    }

    public void Clear()
    {
        string fullPath = Path.Combine(path, fileName);
        File.Delete(fullPath);
    }

    string EncryptDecrypt(string data)
    {
        string newData = "";
        for (int i = 0; i < data.Length; i++)
        {
            newData += (char) (data[i] ^ codeWord[i % codeWord.Length]);
        }
        return newData;
    }
}

