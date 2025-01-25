using System.Collections;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    public static IndicatorManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowMovementIndicator(Vector2 position, Enemy enemyData)
    {
        if (enemyData.movementIndicatorPrefab == null)
        {
            Debug.LogWarning("Movement Indicator Prefab not assigned in Enemy Data.");
            return;
        }

        GameObject indicator = Instantiate(enemyData.movementIndicatorPrefab, position, Quaternion.identity);
        Indicator indicatorScript = indicator.GetComponent<Indicator>();
        if (indicatorScript != null)
        {
            StartCoroutine(indicatorScript.Initialize(enemyData.movementIndicatorColor, enemyData.movementIndicatorDuration, enemyData.movementIndicatorPrefab));
        }
        else
        {
            Debug.LogWarning("Movement Indicator Prefab does not have an Indicator script attached.");
        }
    }
}