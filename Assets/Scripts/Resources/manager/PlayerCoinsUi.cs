using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCoinsUi : MonoBehaviour
{
    private Coins _coins;

    private TextMeshProUGUI _coinsPreview;
    [SerializeField] private float AnimationDuration;

    private void Awake(){
        _coinsPreview = GetComponentInChildren<TextMeshProUGUI>();

    }

    private void Start(){
        _coins = GameManager.Instance.coins;
        _coinsPreview.text = _coins.Value.ToString();
        _coins.OnValueChanged += UpdateCoinsUI;
    }

    private void UpdateCoinsUI(int newValue){
        StopAllCoroutines();
        StartCoroutine(AnimateCoins(newValue));
    }

    private IEnumerator AnimateCoins(int newValue){
        SmoothValueAnimator<int> coinsAnimator = new(_coins.Value, newValue, AnimationDuration);

        while(coinsAnimator.IsRunning()){
            coinsAnimator.Update();
            _coinsPreview.text = coinsAnimator.CurrentValue.ToString();
            yield return null;
        }
        _coinsPreview.text = newValue.ToString();
    }

    private void OnDestroy(){
        _coins.OnValueChanged -= UpdateCoinsUI;
    }

}