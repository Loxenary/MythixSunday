using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Indicator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Enemy EnemyData;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator Initialize(Color color, float duration, GameObject indicator)
    {
        float elapsedTime = 0f;

        Color startColor = new Color(color.r,color.g,color.b,0/5f);
        Color endColor = new (color.r,color.g,color.b,1f);

        while (elapsedTime < duration)
        {
            // Interpolate the color based on the elapsed time
            float t = elapsedTime / duration;
            spriteRenderer.color = Color.Lerp(startColor, endColor, t);

            // Wait for the next frame
            elapsedTime += Time.deltaTime;
            yield return null;

            // Ensure the final color is set
             if (indicator != null && spriteRenderer != null)
            {
                spriteRenderer.color = endColor;
            }
   
        }
        Destroy(gameObject);
    }

    public void StartShowingIndicator(Color color, float duration, GameObject indicator){
        StartCoroutine(Initialize(color, duration, indicator));
    }
}