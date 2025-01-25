using System;
using UnityEngine;


public interface IKey{
    public bool IsDestructable { get;}
    public KeyCode Key {get; }

    public bool ReduceHealth(int amount);

    public Character Character {get; set;}
}