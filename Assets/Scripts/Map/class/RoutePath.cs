using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoutePath : MonoBehaviour
{
    public List<Transform> transformPoints = new(); // Control points for the path

    public static Action onfinishLoadingEvent;

    private void Start()
    {
        if(transformPoints == null || transformPoints.Count <= 0){
            transformPoints = GetComponentsInChildren<Transform>().ToList().
            FindAll(tr => tr != this.GetComponent<Transform>());
            onfinishLoadingEvent.Invoke();
        }
    }

    // Draw the path in the editor for visualization
    private void OnDrawGizmos()
    {
        
        List<Vector3> points = TransformsToListVector3(transformPoints);
        if (points.Count < 2) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < points.Count - 1; i++)
        {
            Gizmos.DrawLine(points[i], points[i + 1]);
        }
    }

    private List<Vector3> TransformsToListVector3(List<Transform> listTrans){
        if(listTrans == null){}
        List<Vector3> points = new();
        foreach(Transform t in listTrans){
            if(t == null){
                continue;
            }
            points.Add(t.position);
        }
        return points;
    }

    // Get the position along the path based on a normalized distance (0 to 1)
    public Vector3 GetPositionAlongPath(float t)
    {
        List<Vector3> points = TransformsToListVector3(transformPoints);
        if (points.Count == 0)
            return Vector3.zero;

        t = Mathf.Clamp01(t); // Ensure t is between 0 and 1
        int segment = Mathf.FloorToInt(t * (points.Count - 1));
        float segmentT = t * (points.Count - 1) - segment;

        if (segment >= points.Count - 1)
            return points[points.Count - 1];

        return Vector3.Lerp(points[segment], points[segment + 1], segmentT);
    }
}