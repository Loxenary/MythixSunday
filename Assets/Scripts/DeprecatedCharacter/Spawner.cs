using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<Character> charactersReferences;   

    private float _spawnInterval;
    
    public void IncreaseIntervalTime(float time){
        _spawnInterval += time;
    }

    public void DecreaseIntervalTime(float time){
        _spawnInterval -= time;
        
    }


    

}