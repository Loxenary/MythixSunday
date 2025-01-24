using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopsManager : MonoBehaviour
{
    public List<IKey> AvailableCharacters;

    [SerializeField] private GameObject keyPrefab;

    public RoutePath altPath;
    public RoutePath f4Path; 

    private Troops altTroop;
    private Troops f4Troop;

    [SerializeField] private MainKey altMainKey;
    [SerializeField] private MainKey f4MainKey;

    public float spawnInterval = 3f;  

    private void Start(){
        InputManager.Instance.onKeyPressed += HandleKeyPress;
        altTroop = new Troops(altMainKey, altPath);
        f4Troop = new Troops(f4MainKey, f4Path);
        StartCoroutine(SpawnTroops());
    }

    private IEnumerator SpawnTroops(){
        while(true){
            SpawnTroop(altTroop);
            SpawnTroop(f4Troop);
            

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnTroop(Troops troop){
        int troopSize = Random.Range(1,6);
        List<IKey> charactersForThisBatch = new (AvailableCharacters);

        for(int i = 0; i < troopSize; i++){
            if(charactersForThisBatch.Count >  0 ){
                int randomIndex = Random.Range(0, charactersForThisBatch.Count);
                IKey randomCharacter = charactersForThisBatch[randomIndex];
                troop.SpawnKey(keyPrefab, randomCharacter);
            }
            else{
                troop.SpawnKey(keyPrefab, AvailableCharacters[0]);
            }
        }
    }

    private void HandleKeyPress(KeyCode key){
        // Check Alt's troop
        if (altTroop.HandleKeyPress(key))
        {
            Debug.Log("Alt's sequence progressed!");
        }

        // Check F4's troop
        if (f4Troop.HandleKeyPress(key))
        {
            Debug.Log("F4's sequence progressed!");
        }
    }

    private void OnDestroy()
    {  
       InputManager.Instance.onKeyPressed -= HandleKeyPress; 
    }
}