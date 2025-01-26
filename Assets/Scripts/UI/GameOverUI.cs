using TMPro;
using UnityEngine;
public class GameOverUI : MonoBehaviour, ISaveLoad
{

    [SerializeField] private TextMeshProUGUI coinsPreview;

    private ResourcesSaveData _saveData;
    public void GoToShop(){
        MySceneManager.Instance.LoadSceneWithMusic(SceneEnum.SHOP, "ShopBGM");
    }

    public void MainMenu(){
        MySceneManager.Instance.LoadSceneWithMusic(SceneEnum.MAIN_MENU, "MainMenuBGM");
    }

    private void Start(){
        GameManager.Instance.OnGameOver += SetupUI;
        gameObject.SetActive(false);
    }

    private void SetupUI(){
        Load();
        gameObject.SetActive(!gameObject.activeInHierarchy);
        Time.timeScale = 0; 
        if(coinsPreview == null){
            coinsPreview = GetComponentInChildren<TextMeshProUGUI>();
        }
        if(_saveData == default || _saveData == null){
            coinsPreview.text = GameManager.Instance.coins.Value.ToString();
    }else{
            coinsPreview.text = _saveData.Coins + GameManager.Instance.coins.Value.ToString();
        }
        
        Save();
    }

    public void Save()
    {
        if(_saveData == default || _saveData == null){
            _saveData = new ResourcesSaveData{
                Coins = GameManager.Instance.coins.Value,
            };
        }else{
            _saveData.Coins = GameManager.Instance.coins.Value  + _saveData.Coins;
        }
        
        
        SaveLoadManager.Save(_saveData);
    }

    public void Load()
    {
        _saveData = SaveLoadManager.Load<ResourcesSaveData>();
    }
}
