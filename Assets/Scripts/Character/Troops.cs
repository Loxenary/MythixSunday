using System.Collections.Generic;
using UnityEngine;

enum TroopsPositionEnum{
    RIGHT,
    LEFT
}

public class Troops : MonoBehaviour
{
    private List<Character> _characterTroops;
    
    private List<KeyCode> _stacks;

    private TroopsPositionEnum _position;

    public void AddCharacter(Character character){
        _characterTroops.Add(character);
    }

    public void HandleUserInput(KeyCode keycode){
        int firstIndex = _position == TroopsPositionEnum.RIGHT ? 0 : 1;
    }
}