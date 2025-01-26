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
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button storeButton;

    [SerializeField] private PauseUI pauseUI;

    private  readonly List<GameObject> _panels = new();

    private void Start(){
        
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

    private void OnPlayButtonClicked(){
        SceneManager.LoadScene((int)SceneEnum.GAME);
    }

    //Turn on only the selected Button
    private void SetActivePanel(GameObject panel){
        foreach(GameObject obj in _panels){
            
            if(obj == panel){
                obj.SetActive(true);
            }else{
                obj.SetActive(false);
            }
        }
    }

    private void OpenStore(){
        SceneManager.LoadScene((int)SceneEnum.SHOP);
    }
}
