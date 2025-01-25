using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public static CharacterControl Instance { get; private set;}
    private List<Character> _altTroops;
    
    private List<Character> _inputSequence = new();

    private Character _altTroopChar;

    private void Awake()
    {
        if(Instance != null && Instance != this){
            Destroy(Instance);
        }
        Instance = this;
        DontDestroyOnLoad(Instance);
    }

    public void AddCharacterToLeft(Character character)
    {
        _altTroops.Add(character);
    }

    
}