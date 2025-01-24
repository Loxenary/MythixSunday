using UnityEngine;

public interface IKey{
    public bool IsDestructable { get;}
    public KeyCode Key {get; }

    public void Delete();
}