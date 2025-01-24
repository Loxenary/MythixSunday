using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform inventoryPanel; // Parent object for inventory slots
    [SerializeField] private GameObject inventorySlotPrefab; // Prefab for an inventory slot    

    private List<UIInventorySlot> _inventorySlots = new (); // List of UI inventory slots

    [SerializeField] private DraggedObject draggedObjectPlaceholder;


    private void Start(){
        Initialize(InventoryManager.Instance.GetInventorySlots().Count);
        InventoryManager.Instance.onInventoryChanged += UpdateUI;
    }

    private void OnDestroy()
    {
        InventoryManager.Instance.onInventoryChanged -= UpdateUI;
    }

    // Initialize the UI inventory
    public void Initialize(int inventorySize)
    {
        // Clear existing slots
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        // Create slots for the inventory
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);
            _inventorySlots.Add(slot.GetComponent<UIInventorySlot>());
            _inventorySlots[i].SetDraggedObject(draggedObjectPlaceholder);
        }

        UpdateUI();
    }

    

    // Update the UI to reflect the current inventory
    public void UpdateUI()
    {
        List<InventorySlot> inventorySlots = InventoryManager.Instance.GetInventorySlots();
        Debug.Log(inventorySlots);
        for (int i = 0; i < _inventorySlots.Count; i++)
        {
            if (i < inventorySlots.Count)
            {
                _inventorySlots[i].SetInventorySlot(inventorySlots[i]); // Set the item in the slot
            }
            else
            {
                _inventorySlots[i].ClearSlot(); // Clear the slot if no item is present
            }
        }
    }
}