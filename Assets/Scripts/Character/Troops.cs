using System.Collections.Generic;
using UnityEngine;

public class Troops
{
    public MainKey Character{get; private set;}
    public RoutePath Path { get; private set;}
    public List<NormalKey> Keys { get; private set;}

    public Queue<NormalKey> AttachedKeys {get; private set;}

    public Troops(MainKey character, RoutePath path){
        Character = character;
        Path = path;
        Keys = new ();
    }
    
    public void SpawnKey(GameObject keyPrefab,IKey keyCharacter){
        int troopSize = Random.Range(1,6);
         for (int i = 0; i < troopSize; i++)
        {
            // Spawn the key at the starting point of the path
            Vector3 spawnPosition = Path.GetPositionAlongPath(0); // Start at the beginning of the path
            Object.Instantiate(keyPrefab, spawnPosition, Quaternion.identity);
            IKey key = keyPrefab.GetComponent<IKey>();

            // Assign the character to the key
            key = keyCharacter;


            // Set the key to normal mode (not attached) unless it's the first key and no attached key exists
             if (key is NormalKey normalKey)
            {
                if (Random.Range(0, 2) == 0) // 50% chance to make it attached
                {
                    normalKey.IsAttached = true;
                    AttachedKeys.Enqueue(normalKey); // Add to the queue of attached keys
                    
                }
                else
                {
                    normalKey.IsAttached = false;
                }
                Keys.Add(normalKey);
            }
        }
    }

    public bool HandleKeyPress(KeyCode key){

        for (int i = Keys.Count - 1; i >= 0; i--){
            if(Keys[i].Key != key){
                continue;
            }
            if(!Keys[i].IsAttached){
                if(Keys[i].ReduceHealth(1)){
                    Keys.RemoveAt(i);
                }
                return true;
            }
            if(AttachedKeys.Count > 0 && Keys[i] == AttachedKeys.Peek()){
                if(Keys[i].ReduceHealth(1)){
                    AttachedKeys.Dequeue();
                    Keys.RemoveAt(i);
                }
                return true;
            }
        }
        

        return false;
    }

    public void ResetSequence(){
        AttachedKeys.Clear();
    }
}