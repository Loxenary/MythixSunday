using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject{
    [Header("Item Details")]

    public string itemName = "New Item";
    public Sprite icon = null;
    public bool isStackable = false;

    [SerializeField]
    private int _maxStackSize = 1;

    public int MaxStackSize{
        get{return _maxStackSize;}
        set{
            if(isStackable){
                _maxStackSize = Mathf.Max(1,value);
            }
            else{
                _maxStackSize = 1;
            }
        }
    }

    [TextArea]
    public string description = "Item Description";


    public virtual void Use(){

    }
}