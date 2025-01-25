using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TroopsManager : MonoBehaviour
{
    public List<Character> AvailableCharacters;
    public static List<Character> S_SpawnedCharacters = new();
    [SerializeField] private GameObject keyPrefab;

    [SerializeField] private float FirstSpawnDurationOffset = 2f;
    

    public RoutePath altPath;
    public RoutePath f4Path; 

    private Troops altTroop;
    private Troops f4Troop;

    [SerializeField] private MainKey altMainKey;
    [SerializeField] private MainKey f4MainKey;

    public float SpawnInterval = 4f;  
    private float SpawnIntervalBetweenRandomLoops = 1f;

    private void Start(){
        InputManager.Instance.onKeyPressed += HandleKeyPress;
        LoadKeysFromResources();
        RoutePath.onfinishLoadingEvent += SetupMainTroopsAndKeysMovement;
    }

    private void SetupMainTroopsAndKeysMovement(){
        if(!altMainKey || !f4MainKey){
            Debug.LogWarning("Key not setup yet");
            return;
        }
        altTroop = new Troops(altPath);
        f4Troop = new Troops(f4Path);
        Movement altMovement = altMainKey.gameObject.GetComponent<Movement>();
        Movement f4Movement = f4MainKey.gameObject.GetComponent<Movement>();

        altMainKey.SetTroops(altTroop);
        f4MainKey.SetTroops(f4Troop);

        altMainKey.Character.ResetSpeed();
        f4MainKey.Character.ResetSpeed();

        if(!altMovement || !f4Movement){
            Debug.LogWarning("Movement script aren't set in the Keys");
            return;
        }
        altMovement.Initialize(altPath.transformPoints);
        f4Movement.Initialize(f4Path.transformPoints);
        StartCoroutine(SpawnTroops());
    }


    private void LoadKeysFromResources(){
        Character[] loadedKeys = Resources.LoadAll<Character>("Character");
        AvailableCharacters = loadedKeys.ToList();
        
    }

    private IEnumerator SpawnTroops(){
        yield return new WaitForSeconds(FirstSpawnDurationOffset);
        while(true){
            int troopSize = Random.Range(1,6);
            for(int i = 0; i < troopSize; i++){
                SpawnTroop(altTroop);
                SpawnTroop(f4Troop);
                
                yield return new WaitForSeconds(SpawnIntervalBetweenRandomLoops);
            }
            yield return new WaitForSeconds(SpawnInterval);
        }
        
    }

    private void SpawnTroop(Troops troop){        
        List<Character> charactersForThisBatch = new (AvailableCharacters.FindAll(character => !S_SpawnedCharacters.Contains(character)));
        if(charactersForThisBatch.Count >  0 ){
            int randomIndex = Random.Range(0, charactersForThisBatch.Count);
            Character randomCharacter = charactersForThisBatch[randomIndex];
            S_SpawnedCharacters.Add(randomCharacter);
            troop.SpawnKey(keyPrefab, randomCharacter);
        }
        else{
            troop.SpawnKey(keyPrefab, AvailableCharacters[0]);
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