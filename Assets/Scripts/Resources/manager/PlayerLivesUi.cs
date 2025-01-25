using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLivesUi : MonoBehaviour
{
    private IntHealth _lives;

    private TextMeshProUGUI _livesPreview;
    [SerializeField] private float AnimationDuration;

    private void Awake(){
        _livesPreview = GetComponentInChildren<TextMeshProUGUI>();

    }

    private void Start(){
        _lives = GameManager.Instance.playerLives;
        _livesPreview.text = _lives.Value.ToString();
        _lives.OnValueChanged += UpdateLivesUI;
    }

    private void UpdateLivesUI(int newValue){
        StopAllCoroutines();
        StartCoroutine(AnimateLives(newValue));
    }

    private IEnumerator AnimateLives(int newValue){
        SmoothValueAnimator coinsAnimator = new(_lives.Value, newValue, AnimationDuration);

        while(coinsAnimator.IsRunning()){
            coinsAnimator.Update();
            _livesPreview.text = coinsAnimator.CurrentValue.ToString();
            yield return null;
        }
        _livesPreview.text = newValue.ToString();
    }

    private void OnDestroy(){
        _lives.OnValueChanged -= UpdateLivesUI;
    }

}