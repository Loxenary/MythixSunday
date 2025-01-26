using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum SceneEnum{
    MAIN_MENU = 0,
    GAME = 1,
    SHOP = 2,
}
public class MySceneManager : MonoBehaviour
{
    public static MySceneManager Instance { get; private set; }
    private void Awake(){
        if(Instance != this && Instance != null){
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(Instance);
    }
    public void LoadScene(SceneEnum sceneEnum
    ){
        SceneManager.LoadScene((int)sceneEnum);
    }   

    public void LoadSceneWithMusic(SceneEnum sceneEnum, string music){
        LoadScene(sceneEnum);
        AudioManager.Instance.PlayMusic(music);
    }    
}


