using UnityEngine;

public class SingleRowGrid : MonoBehaviour
{

    [Tooltip("Space between each item")]
    public float spacing = 2f; // Default spacing
    public float scale = 1f;
    public bool isForcingPosition = false;

    private Movement _movement;

    private void Start()
    {
        _movement = GetComponent<Movement>();
        HandleChildPosition();
        ArrangeChildrenInRow();
    }

    private void HandleChildPosition(){
        if(!isForcingPosition){
            return;
        }

        foreach(var item in transform.GetComponentsInChildren<Transform>()){
            if(item != transform){
                item.localPosition = Vector3.zero;
            }
        }
    }

    private void ArrangeChildrenInRow()
    {
        // Get all child objects
        int childCount = transform.childCount;

        // If no children, return
        if (childCount == 0)
        {
            Debug.LogWarning("No child objects found in the parent GameObject.");
            return;
        }

        // Calculate the total width of the row
        float totalWidth = (childCount - 1) * spacing;

        // Get the starting position (centered)
        Vector3 startPosition = transform.position - new Vector3(totalWidth / 2, 0, 0);
        if(_movement.GetMovingDirection()){
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.position = startPosition + new Vector3(childCount - 1 - i * spacing, 0, 0);
            }
            return;
        }
        
        // Loop through all child objects and set their positions
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.position = startPosition + new Vector3(i * spacing, 0, 0);
        }
    }
}