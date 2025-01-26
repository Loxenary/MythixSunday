using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// This class is used for every single UI element in the main menu
/// </summary>

public class MainMenu : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] public GameObject mainMenuPanel;
    [SerializeField] public GameObject settingsPanel;

    [Header("Buttons")]
    [SerializeField] public Button startButton;
    [SerializeField] public Button settingButton;
    [SerializeField] public Button storeButton;

    [SerializeField] public PauseUI pauseUI;

    public  readonly List<GameObject> _panels = new();

    public void Start(){
        
        _panels.Add(mainMenuPanel);
        _panels.Add(settingsPanel);
        AudioManager.Instance.PlayMusic("MainMenuBGM",true,100);
        startButton.onClick.AddListener(OnPlayButtonClicked);
        settingButton.onClick.AddListener(() => {
            settingsPanel.SetActive(true);
            pauseUI.OpenSettings();
        });
        storeButton.onClick.AddListener(OpenStore);
        SetActivePanel(mainMenuPanel);
    }

    public void OnPlayButtonClicked(){
        MySceneManager.Instance.LoadSceneWithMusic( SceneEnum.GAME,"Stage1BGM");
    }

    //Turn on only the selected Button
    public void SetActivePanel(GameObject panel){
        foreach(GameObject obj in _panels){
            
            if(obj == panel){
                obj.SetActive(true);
            }else{
                obj.SetActive(false);
            }
        }
    }

    public void OpenStore(){
        MySceneManager.Instance.LoadSceneWithMusic( SceneEnum.SHOP,"ShopBGM");
    }
}
