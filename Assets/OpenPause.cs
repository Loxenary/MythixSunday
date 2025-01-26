using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OpenPause : MonoBehaviour
{
    private Button _button;

    [SerializeField] private PauseUI pauseUI;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => pauseUI.OpenSettings());
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape) && !pauseUI.gameObject.activeInHierarchy){
            pauseUI.OpenSettings();
        }
    }
}
