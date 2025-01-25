using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour, ISaveLoad
{

    [SerializeField] private TextMeshProUGUI coinsPreview;

    private ResourcesSaveData _saveData;
    public void RestartGame(){
        GameManager.Instance.RestartGame();
    }

    public void MainMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    private void Start(){
        GameManager.Instance.OnGameOver += SetupUI;
        gameObject.SetActive(false);
    }

    private void SetupUI(){
        gameObject.SetActive(!gameObject.activeInHierarchy);
        Time.timeScale = 0; 
        coinsPreview.text = _saveData.Coins + GameManager.Instance.coins.Value.ToString();
        Save();
    }

    public void Save()
    {
        _saveData.Coins = GameManager.Instance.coins.Value  + _saveData.Coins;
        
        SaveLoadManager.Save(_saveData);
    }

    public void Load()
    {
        _saveData = SaveLoadManager.Load<ResourcesSaveData>();
    }
}
