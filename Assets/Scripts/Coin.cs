using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    public int coinValue = 1;
    public string poolTag = "Coin";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("ALT"))
        {
            // Add coin to player's inventory
            GameManager.Instance.AddCoins(coinValue);

            // Return the coin to the pool instead of destroying it
            ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
        }
    }

    /// <summary>
    /// Resets the coin's state when retrieved from the pool.
    /// </summary>
    private void OnEnable()
    {
        // Reset any necessary properties here
        // Example: Play spawn animation or reset visual states
    }
}