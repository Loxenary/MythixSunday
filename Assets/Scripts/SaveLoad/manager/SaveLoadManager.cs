using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

public static class SaveLoadManager
{
    private static readonly Dictionary<Type, string> _filePaths = new Dictionary<Type, string>();

    // Register a data type with a custom file name
    public static void RegisterDataType<T>(string fileName)
    {
        _filePaths[typeof(T)] = fileName;
    }

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
        T data = new T();
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