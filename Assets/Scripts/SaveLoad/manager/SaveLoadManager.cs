using System.IO;
using UnityEngine;

public static class SaveLoadManager
{

    // Save data of a specific type
    public static void Save<T>(T data) where T : ISaveData
    {
        string fileName = data.FileName;
        string json = JsonUtility.ToJson(data, prettyPrint : true);
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        
        File.WriteAllText(filePath, json);
        Debug.Log($"Saved {typeof(T).Name} to {filePath}");
    }

    // Load data of a specific type
    public static T Load<T>() where T : ISaveData, new()
    {
        T data = new();
        string fileName = data.FileName;
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if(File.Exists(filePath)){
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(json);
        }
        else{
            Debug.LogWarning($"File not found at {filePath}. Returning default data.");
            return data;
        }
    }
}