using System.Collections;
using TMPro;
using UnityEngine;

public class ScorePlayerUI : MonoBehaviour
{
    private Score _score;
    [SerializeField] private TextMeshProUGUI scorePreview;

    [SerializeField] private float AnimationDuration;

    private void Awake()
    {
        if (scorePreview == null)
        {
            scorePreview = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    private void Start()
    {
        _score = GameManager.Instance.Score;
        _score.OnValueChanged += UpdateScoreUI;
    }

    private void UpdateScoreUI(long newValue)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateScore(newValue));
    }

    private IEnumerator AnimateScore(long newValue)
    {
        // Parse the current score text back into a number
        long currentValue = ParseScore(scorePreview.text);

        // Create the animator with the parsed current value
        SmoothValueAnimator<long> scoreAnimator = new(currentValue, newValue, AnimationDuration);

        while (scoreAnimator.IsRunning())
        {
            scoreAnimator.Update();
            scorePreview.text = FormatScore(scoreAnimator.CurrentValue);
            yield return null; // Wait for the next frame
        }

        // Ensure the final value is set
        scorePreview.text = FormatScore(newValue);
    }

    private string FormatScore(long score)
    {
        if (score >= 1000000000000) // 1 trillion (1t)
        {
            return (score / 1000000000000).ToString() + "t";
        }
        else if (score >= 1000000000) // 1 billion (1b)
        {
            return (score / 1000000000).ToString() + "b";
        }
        else if (score >= 1000000) // 1 million (1m)
        {
            return (score / 1000000).ToString() + "m";
        }
        else if (score >= 1000) // 1 thousand (1k)
        {
            return (score / 1000).ToString() + "k";
        }
        else
        {
            return score.ToString(); // No suffix for values less than 1000
        }
    }

    private long ParseScore(string scoreText)
    {
        // Remove any suffix (k, m, b, t) and parse the number
        if (scoreText.EndsWith("t"))
        {
            return long.Parse(scoreText.TrimEnd('t')) * 1000000000000;
        }
        else if (scoreText.EndsWith("b"))
        {
            return long.Parse(scoreText.TrimEnd('b')) * 1000000000;
        }
        else if (scoreText.EndsWith("m"))
        {
            return long.Parse(scoreText.TrimEnd('m')) * 1000000;
        }
        else if (scoreText.EndsWith("k"))
        {
            return long.Parse(scoreText.TrimEnd('k')) * 1000;
        }
        else
        {
            // No suffix, parse as-is   
            return long.Parse(scoreText);
        }
    }

    private void OnDestroy()
    {
        _score.OnValueChanged -= UpdateScoreUI;
    }
}