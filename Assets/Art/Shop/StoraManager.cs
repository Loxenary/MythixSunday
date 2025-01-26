using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoraManager : MonoBehaviour
{
   [SerializeField] private GameObject Panel;
   private Image panelImage; 
    [SerializeField] private Sprite ShopSprite;
    [SerializeField] private Sprite StatsSprite;
    
    [SerializeField] private Button OpenShop;
    [SerializeField] private Button OpenStats;

    [SerializeField] private Button BackToMenu;

    [SerializeField] private TextMeshProUGUI CoinsPreview;

    [SerializeField] private StoreObjects StoreObjects;
    [SerializeField] private GameObject StatsObjects;

    private void Start(){
        OpenShop.onClick.AddListener(OpenShopPage);
        OpenStats.onClick.AddListener(OpenStatsPage);
        BackToMenu.onClick.AddListener(BackToMainMenu);
        panelImage = Panel.GetComponent<Image>();
        Load();
    }

    private void OpenShopPage(){
        StoreObjects.gameObject.SetActive(true);
        StatsObjects.SetActive(false);
        panelImage.sprite = ShopSprite;
    }

    private void BackToMainMenu(){
        MySceneManager.Instance.LoadSceneWithMusic(SceneEnum.MAIN_MENU, "MainMenuBGM");
    }

    private void OpenStatsPage(){
        StoreObjects.gameObject.SetActive(false);
        StatsObjects.SetActive(true);
        panelImage.sprite = StatsSprite;
    }

    private void Load(){
        ResourcesSaveData duit = SaveLoadManager.Load<ResourcesSaveData>();
        if(duit != null && duit != default){
            CoinsPreview.text = duit.Coins.ToString();
        }
        CoinsPreview.text = 0.ToString();
    }
}
