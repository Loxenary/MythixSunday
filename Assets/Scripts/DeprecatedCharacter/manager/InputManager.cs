using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set;}

    public delegate void OnKeyPressed(KeyCode key);
    public event OnKeyPressed onKeyPressed;
    


    public void Awake()
    {
        if(Instance != this && Instance != null){
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void Update(){
        if(Input.anyKeyDown){    
            foreach(KeyCode key in System.Enum.GetValues(typeof(KeyCode))){

            if(Input.GetKeyDown(key)){
                Debug.Log("Pressed Key :" + key);
                onKeyPressed?.Invoke(key);
            }
        }
        }
    }
}