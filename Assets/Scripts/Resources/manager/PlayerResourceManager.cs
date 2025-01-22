using UnityEngine;
using UnityEngine.UI;

public class PlayerResourceManager : MonoBehaviour
{
    public static PlayerResourceManager Instance { get; private set;}
    [SerializeField] private Button TestAddButton;
    [SerializeField] private Button TestReduceButton;

    [SerializeField] private PlayerHealthUI playerHealthUI;

    private void Awake()
    {
        if(Instance != null && Instance != this){
            Destroy(gameObject);
        }
        Instance = this;
    }

    private void Start(){
        TestAddButton.onClick.AddListener(() => playerHealthUI.health.Add(10));
        TestReduceButton.onClick.AddListener(() => playerHealthUI.health.Reduce(10));
    }
}
