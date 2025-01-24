using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set;}

    [SerializeField] private int inventorySize = 20;

    private List <InventorySlot> _inventorySlots = new();
    [SerializeField] List<Item> items= new();

    public delegate void OnInventoryChanged();
    public event OnInventoryChanged onInventoryChanged;

    private void Awake(){
        
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start(){
        if(items != null){
            foreach(var item in items){
                _inventorySlots.Add(new InventorySlot(item, item.MaxStackSize));
            }
        }
    }

    public bool AddItem(Item item, int quantity = 1){
        if(quantity <= 0){
            Debug.LogWarning("Cannot add zero or negative quantity of items");
            return false;
        }

        InventorySlot existingSlot = FindItemSlot(item);
        if(item.isStackable && existingSlot != null){
            HandleAddingSlotItem(existingSlot, quantity);
            onInventoryChanged?.Invoke();
            return true;
        }

        if(_inventorySlots.Count >= inventorySize){
            Debug.LogWarning("Inventory is full!");
            return false;
        }

        _inventorySlots.Add(new InventorySlot(item, quantity));
        Debug.Log($"Added {quantity}x {item.itemName} to inventory.");
        onInventoryChanged?.Invoke();
        return true;
    }

    private void HandleAddingSlotItem(InventorySlot slot, int quantity = 1){
        int addedStack = slot.StackSize + quantity;
        int overFlowingStack = addedStack - slot.Item.MaxStackSize;
        if(overFlowingStack <= 0){
            slot.StackSize += quantity;
            Debug.Log($"Adding slot with id {slot.SlotId} item by {quantity}");
            return;
        }
        if(_inventorySlots.Count >= inventorySize){
            Debug.LogWarning($"Inventory is full");
            return;
        }        

        Debug.Log($"Create new Slot with stack size of {overFlowingStack}");
        slot.StackSize += quantity;
        InventorySlot newSlot = new(slot.Item, overFlowingStack);
        _inventorySlots.Add(newSlot);
    
    }

    public bool RemoveItem(Item item, int quantity = 1){
        if(_inventorySlots.Count == 0){
            Debug.LogWarning("Equipment Is Empty");
            return false;
        }
        InventorySlot existingSlot = FindItemSlot(item);

        if(existingSlot == null){
            Debug.LogWarning($"Item {item.itemName} not found in inventory");
            return false;
        }

        if(item.isStackable){                        
            // Only Reduce stack size if not yet reach 0
            // if item is empty then just remove from slots
            existingSlot.StackSize -= quantity;
            if(existingSlot.StackSize <= 0){
                _inventorySlots.Remove(existingSlot);                
            }
            onInventoryChanged?.Invoke();
            return true;
        }
        _inventorySlots.Remove(existingSlot);
        onInventoryChanged?.Invoke();
        return true;
    }

    public void SwapItems(int slotIdA, int slotIdB)
    {
        InventorySlot slotA = FindItemSlot(slotIdA);
        InventorySlot slotB = FindItemSlot(slotIdB);

        if (slotA != null && slotB != null)
        {
            Item tempItem = slotA.Item;
            int tempStackSize = slotA.StackSize;

            slotA.SetItem(slotB.Item);
            slotA.StackSize = slotB.StackSize;

            slotB.SetItem(tempItem);
            slotB.StackSize = tempStackSize;

            onInventoryChanged?.Invoke();
        }
    }

    public InventorySlot FindItemSlot(int slotId)
    {
        return _inventorySlots.Find(slot => slot.SlotId == slotId);
    }


    public List<InventorySlot> GetInventorySlots() => _inventorySlots;

    private InventorySlot FindItemSlot(Item item){
        return _inventorySlots.Find(slot => slot.Item == item);
    }

}