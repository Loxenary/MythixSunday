using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    private static int s_slotId = 0;
    public int SlotId { get; set; }
    public Item Item { get; private set;}

    public int StackSize;

    public InventorySlot(Item item, int stackSize)
    {
        SlotId = s_slotId;
        s_slotId++;
        Item = item;
        StackSize = stackSize;
    }

    public void SetItem(Item item) {
        if(item != null){
            Item = item;
        }
    }
}