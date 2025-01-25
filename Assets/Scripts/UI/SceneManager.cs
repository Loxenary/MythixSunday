using UnityEngine;
using UnityEngine.SceneManagement;


public enum SceneEnum{
    MAIN_MENU = 1,
    GAME = 2,
    GAME_OVER = 3,
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
    
}
