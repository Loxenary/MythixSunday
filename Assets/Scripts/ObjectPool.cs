using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Singleton instance
    public static ObjectPool Instance;

    // Pool group to manage different pools
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    // Prefabs and initial sizes can be configured via the Inspector
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int initialSize;
    }

    public List<Pool> pools;

    void Awake()
    {
        // Initialize the singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize the pool dictionary
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // Create pools based on the configured pools list
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.initialSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    /// <summary>
    /// Retrieves an object from the pool associated with the specified tag.
    /// If the pool is empty, it dynamically expands by instantiating a new object.
    /// </summary>
    /// <param name="tag">The tag identifying the pool.</param>
    /// <param name="position">Position to spawn the object.</param>
    /// <param name="rotation">Rotation to spawn the object.</param>
    /// <returns>The pooled GameObject.</returns>
    public GameObject GetFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"[ObjectPool] Pool with tag '{tag}' doesn't exist.");
            return null;
        }

        GameObject obj;

        if(poolDictionary[tag].Count > 0)
        {
            obj = poolDictionary[tag].Dequeue();
        }
        else
        {
            // Dynamically expand the pool
            GameObject prefab = GetPrefabByTag(tag);
            if(prefab == null)
            {
                Debug.LogError($"[ObjectPool] No prefab found for tag '{tag}'.");
                return null;
            }

            obj = Instantiate(prefab);
            Debug.Log($"[ObjectPool] Expanded pool for tag '{tag}' by instantiating a new object.");
        }

        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }

    /// <summary>
    /// Returns an object to its respective pool.
    /// </summary>
    /// <param name="tag">The tag identifying the pool.</param>
    /// <param name="obj">The GameObject to return.</param>
    public void ReturnToPool(string tag, GameObject obj)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"[ObjectPool] Pool with tag '{tag}' doesn't exist.");
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
    }

    /// <summary>
    /// Retrieves the prefab associated with a given tag.
    /// </summary>
    /// <param name="tag">Tag of the pool.</param>
    /// <returns>Prefab GameObject.</returns>
    private GameObject GetPrefabByTag(string tag)
    {
        foreach(Pool pool in pools)
        {
            if(pool.tag == tag)
            {
                return pool.prefab;
            }
        }
        return null;
    }
}