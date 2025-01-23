using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("UI References")]
    [SerializeField] private Image iconImage; // Reference to the icon Image
    [SerializeField] private TextMeshProUGUI stackSizeText; // Reference to the stack size Text

    private Transform _draggingCanvasTransform; // Canvas transform for dragging
    private GameObject _draggedItem; // The dragged item GameObject
    
    private InventorySlot _inventorySlot;
    public InventorySlot InventorySlot { get{
        return _inventorySlot;
    } private set{
        _inventorySlot = value;
    } } // The item in this slot
    public int StackSize { get; private set; } // The stack size of the item

    // Set the item in the slot
    public void SetInventorySlot(InventorySlot slot){
        Item item = slot.Item;
        int stackSize = slot.StackSize;
        if (item != null)
        {
            _inventorySlot = slot;
            iconImage.sprite = item.icon; // Set the item icon
            iconImage.enabled = true; // Enable the icon

            if (item.isStackable && stackSize > 1)
            {
                stackSizeText.text = stackSize.ToString(); // Set the stack size text
                stackSizeText.enabled = true; // Enable the stack size text
            }
            else
            {
                stackSizeText.enabled = false; // Disable the stack size text
            }
        }
        else
        {
            ClearSlot(); // Clear the slot if no item is provided
        }
    }

    private void Awake()
    {  
       enabled = true;
       GetComponent<Image>().enabled = true;
       iconImage.enabled = true;
       stackSizeText.enabled = true; 
    }

    // Clear the slot
    public void ClearSlot()
    {
        InventorySlot.SetItem(null);
        StackSize = 0;
        iconImage.sprite = null;
        iconImage.enabled = false;
        stackSizeText.enabled = false;
        Debug.Log("IS THIS CALLED");
    }

    // Handle the start of dragging
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (InventorySlot.Item == null) return; // Don't drag if the slot is empty

        // Create a new GameObject for the dragged item
        _draggedItem = new GameObject("Dragged Item");
        _draggedItem.transform.SetParent(_draggingCanvasTransform);

        // Add an Image component to the dragged item
        Image draggedImage = _draggedItem.AddComponent<Image>();
        draggedImage.sprite = iconImage.sprite;
        draggedImage.raycastTarget = false; // Disable raycast target to avoid blocking other UI elements

        // Set the canvas for dragging
        _draggingCanvasTransform = GetComponentInParent<Canvas>().transform;

        // Hide the original icon
        iconImage.enabled = false;
    }

    // Handle dragging
    public void OnDrag(PointerEventData eventData)
    {
        if (_draggedItem == null) return;

        // Move the dragged item with the mouse
        _draggedItem.transform.position = eventData.position;
    }

    // Handle the end of dragging
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_draggedItem == null) return;

        // Destroy the dragged item GameObject
        Destroy(_draggedItem);

        // Show the original icon
        iconImage.enabled = true;

        // Check if the item was dropped on another slot
        // No Object found
        if(eventData.pointerCurrentRaycast.gameObject == null){
            
            Debug.LogWarning("No valid drop target detected.");
            return;
        }

        UIInventorySlot targetInventorySlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>();

        if(targetInventorySlot == null){
            Debug.LogWarning("Dropped on a non-slot object.");
            return;
        }

        Debug.Log($"Swapping between Inventory with id {InventorySlot.SlotId} And {targetInventorySlot.InventorySlot.SlotId}");        
        InventoryManager.Instance.SwapItems(InventorySlot.SlotId, targetInventorySlot.InventorySlot.SlotId);
    }

    // Handle clicking on the slot
    public void OnPointerClick(PointerEventData eventData)
    {
        string itemName = InventorySlot.Item.itemName;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log($"Left-clicked on {itemName}");
            // Add left-click functionality here (e.g., use the item)
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log($"Right-clicked on {itemName}");
            // Add right-click functionality here (e.g., split the stack)
        }
    }
}