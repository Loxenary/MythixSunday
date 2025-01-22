using System;
using UnityEngine;

public static class SaveEventManager
{
    public static event Action OnSaveRequested;
    public static event Action OnLoadRequested;

    public static void Save(){
        OnSaveRequested?.Invoke();
    }

    public static void Load(){
        OnLoadRequested?.Invoke();
    }
}