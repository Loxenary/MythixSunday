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
    [SerializeField] private Button exitButton;

    [Header("Others")]
    private Settings settings;

    private List<GameObject> _panels;

    private void Start(){
        _panels.Add(mainMenuPanel);
        _panels.Add(settingsPanel);

        startButton.onClick.AddListener(OnPlayButtonClicked);
        settingButton.onClick.AddListener(ShowSettings);

        
        settings.BackButtonPressed += ShowMainMenu;
    }

    private void OnPlayButtonClicked(){
        SceneManager.LoadScene("GameScene");
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
    

    private void ShowMainMenu(){
        SetActivePanel(mainMenuPanel);
    }

    private void ShowSettings(){
        SetActivePanel(settingsPanel);
    }

    private void ExitGame(){
        Application.Quit();
    }

}
