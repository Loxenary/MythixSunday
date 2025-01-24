using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI References")]
    [SerializeField] private Image iconImage; // Reference to the icon Image
    [SerializeField] private TextMeshProUGUI stackSizeText; // Reference to the stack size Text

    
    private InventorySlot _inventorySlot;
    public InventorySlot InventorySlot { get{
        return _inventorySlot;
    } private set{
        _inventorySlot = value;
    } } // The item in this slot

    private DraggedObject _draggedObject;

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

    public void SetDraggedObject(DraggedObject draggedObject){
        _draggedObject = draggedObject; 
        _draggedObject.canvasParent = transform.parent as RectTransform;
    } 

    // Handle the start of dragging
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (InventorySlot.Item == null) return; // Don't drag if the slot is empty
        if (_draggedObject.objectPlaceholder == null)
        {
            Debug.LogWarning("Dragged object placeholder isn't set yet");
            return;
        }
        if (_draggedObject.canvasParent == null)
        {
            Debug.LogWarning("Canvas parent of dragged object isn't set yet");
            return;
        }

        _draggedObject.SetImage(iconImage);

        // Disable raycast target to avoid blocking other UI elements
        _draggedObject.Image.raycastTarget = false;
        // Set the initial position of the dragged object using the new function
        _draggedObject.objectPlaceholder.localPosition = GetLocalPositionFromScreen(eventData, _draggedObject.canvasParent as RectTransform);

        // Debug the sprite
        Debug.Log($"Sprite of Dragged Item: {_draggedObject.Image.sprite}");

        // Hide the original icon
        _draggedObject.Image.sprite = iconImage.sprite;
        _draggedObject.previewObject.gameObject.SetActive(true);
        iconImage.enabled = false;
    }

    // Handle dragging
    public void OnDrag(PointerEventData eventData)
    {
        if (_draggedObject.objectPlaceholder == null) return;

        // Move the dragged item with the mouse
        PointerEventData temp = eventData;
        _draggedObject.previewObject.localPosition = GetLocalPositionFromScreen(temp, _draggedObject.canvasParent as RectTransform);
        _draggedObject.objectPlaceholder.localPosition = eventData.position;
        _draggedObject.objectPlaceholder.position = eventData.position;

        Debug.Log($"Pos : {_draggedObject.objectPlaceholder.localPosition} : {eventData.position}");
    }

    // Handle the end of dragging
        // Handle the end of dragging
    public void OnEndDrag(PointerEventData eventData)
    {
        if (_draggedObject.objectPlaceholder == null) return;

        // Disable the draggedItemPlaceholder image        
        _draggedObject.previewObject.gameObject.SetActive(false);
        // Show the original icon
        iconImage.enabled = true;

        // Check if the item was dropped on another slot
        if (eventData.pointerCurrentRaycast.gameObject == null)
        {
            Debug.LogWarning("No valid drop target detected.");
            return;
        }

        UIInventorySlot targetInventorySlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>();

        if (targetInventorySlot == null)
        {
            Debug.LogWarning("Dropped on a non-slot object.");
            return;
        }

        Debug.Log($"Swapping between Inventory with id {InventorySlot.SlotId} And {targetInventorySlot.InventorySlot.SlotId}");
        InventoryManager.Instance.SwapItems(InventorySlot.SlotId, targetInventorySlot.InventorySlot.SlotId);
    }

    // Handle clicking on the slot
    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     string itemName = InventorySlot.Item.itemName;
    //     if (eventData.button == PointerEventData.InputButton.Left)
    //     {
    //         Debug.Log($"Left-clicked on {itemName}");
    //         // Add left-click functionality here (e.g., use the item)
    //     }
    //     else if (eventData.button == PointerEventData.InputButton.Right)
    //     {
    //         Debug.Log($"Right-clicked on {itemName}");
    //         // Add right-click functionality here (e.g., split the stack)
    //     }
    // }
    private Vector2 GetLocalPositionFromScreen(PointerEventData eventData, RectTransform targetRectTransform)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            targetRectTransform, // The target RectTransform (e.g., canvas or parent UI element)
            eventData.position, // The screen position of the mouse
            eventData.pressEventCamera, // The camera used for the canvas (null for overlay canvases)
            out Vector2 localPoint // The output local position
        );
        return localPoint;
    }
}