using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set;}

    private void Awake()
    {   
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {   
        // SaveEventManager.OnSaveRequested += SaveGame;
        // SaveEventManager.OnLoadRequested += LoadGame;
    }
    private void OnDisable()
    {
        // SaveEventManager.OnSaveRequested -= SaveGame;
        // SaveEventManager.OnLoadRequested -= LoadGame;
    }

    #region Save and Load

    public void SaveGame(){
        
    }
    #endregion
}
