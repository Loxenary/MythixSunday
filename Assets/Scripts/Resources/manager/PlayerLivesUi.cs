using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLivesUi : MonoBehaviour
{
    private IntHealth _lives;
    [SerializeField] private float AnimationDuration;
    [SerializeField] private GameObject LivesIconPrefab;

    private void Start(){
        _lives = GameManager.Instance.playerLives;
        _lives.OnValueChanged += UpdateLivesUI;
        GenerateChild(GameManager.Instance.playerLives.Value);
    }

    private void GenerateChild(int newValue){
        // Destroy all existing children
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate new children
        for (int i = 0; i < newValue; i++)
        {
            Instantiate(LivesIconPrefab, transform);
        }
    }

    private void UpdateLivesUI(int newValue){   
        GenerateChild(newValue);
    }

    private void OnDestroy(){
        _lives.OnValueChanged -= UpdateLivesUI;
    }

}