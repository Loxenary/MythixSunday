using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Troops
{
    public RoutePath Path { get; private set;}
    public List<NormalKey> Keys { get; private set;}

    public event Action<float> OnSpeedDecreaseEvent;
    public event Action<float> OnSpeedIncreaseEvent;

    private const float GapDistance = 1.5f;

    public Queue<NormalKey> AttachedKeys  {get; private set;}

    private float _speedMultiplier = 1.2f;

    public Troops(RoutePath path){
        
        Path = path;
        AttachedKeys = new();
        Keys = new ();
    }
    
    private void Enqueue(NormalKey newKey){
        
        AttachedKeys.Enqueue(newKey);
        foreach (var key in AttachedKeys){
            key.Character.MovementSpeed *= _speedMultiplier;
        }
        OnSpeedIncreaseEvent?.Invoke(_speedMultiplier);
        
    }

    private void Dequeue(){
        AttachedKeys.Dequeue();
        if (AttachedKeys.Count == 0)
            return;
        foreach (var key in AttachedKeys){
            key.Character.MovementSpeed /= _speedMultiplier;
        }
        OnSpeedDecreaseEvent?.Invoke(_speedMultiplier);
    }

    public void SpawnKey(GameObject keyPrefab,Character keyCharacter){
        int troopSize = UnityEngine.Random.Range(1,2);
         for (int i = 0; i < troopSize; i++)
        {
            // Spawn the key at the starting point of the path
        Vector3 spawnPosition = Path.GetPositionAlongPath(0); // Start at the beginning of the path
            GameObject obj = UnityEngine.Object.Instantiate(keyPrefab, spawnPosition, Quaternion.identity);
            Movement movement = obj.GetComponent<Movement>();
            // Assign the character to the key
            movement.Key = obj.GetComponent<NormalKey>();
            movement.Key.Character = keyCharacter;
            movement.Initialize(Path.transformPoints);
            movement.enabled = true;


            // Set the key to normal mode (not attached) unless it's the first key and no attached key exists
             if (movement.Key is NormalKey normalKey)
            {
                Keys.Add(normalKey);
            }
        }
    }

    public void AttachNormalKey(NormalKey normalKey){
        if(normalKey.IsAttached){
            return;
        }

        normalKey.IsAttached = true;
        Enqueue(normalKey);
    }

    public void UpdateAttachedKeys(Vector3 mainKeyPosition)
{
    if (AttachedKeys.Count == 0)
        return;

    List<Vector3> pathPoints = Path.TransformsToListVector3(Path.transformPoints);

    // Find the closest point on the path to the MainKey
    int closestIndex = FindClosestPointIndex(mainKeyPosition, pathPoints);

    // Calculate the distance along the path up to the closest point
    float distanceAlongPath = CalculateDistanceAlongPath(pathPoints, closestIndex);

    // Calculate the target position for each attached key along the path
    int index = 0;
    foreach (var attachedKey in AttachedKeys)
    {
        // Calculate the target distance along the path (mainKeyPosition - gap)
        float targetDistance = distanceAlongPath - (GapDistance * (index + 1));

        // Ensure the target distance is not negative
        targetDistance = Mathf.Max(targetDistance, 0);

        // Find the point along the path at the target distance
        Vector3 targetPosition = GetPointAtDistance(pathPoints, targetDistance);

        // Smoothly move the attached key to the target position
        attachedKey.transform.position = Vector3.Lerp(
            attachedKey.transform.position,
            targetPosition,
            Time.deltaTime * attachedKey.Character.MovementSpeed
        );

        index++;
    }
}

    private int FindClosestPointIndex(Vector3 position, List<Vector3> pathPoints)
    {
        int closestIndex = 0;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < pathPoints.Count; i++)
        {
            float distance = Vector3.Distance(position, pathPoints[i]);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }


    private float CalculateDistanceAlongPath(List<Vector3> pathPoints, int upToIndex)
    {
        float distance = 0f;

        for (int i = 0; i < upToIndex; i++)
        {
            distance += Vector3.Distance(pathPoints[i], pathPoints[i + 1]);
        }

        return distance;
    }

    private Vector3 GetPointAtDistance(List<Vector3> pathPoints, float targetDistance)
    {
        float accumulatedDistance = 0f;

        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            float segmentDistance = Vector3.Distance(pathPoints[i], pathPoints[i + 1]);

            if (accumulatedDistance + segmentDistance >= targetDistance)
            {
                // Interpolate between the two points to find the exact position
                float t = (targetDistance - accumulatedDistance) / segmentDistance;
                return Vector3.Lerp(pathPoints[i], pathPoints[i + 1], t);
            }

            accumulatedDistance += segmentDistance;
        }

        // If the target distance is beyond the path, return the last point
        return pathPoints[pathPoints.Count - 1];
    }    

    public bool HandleKeyPress(KeyCode key){

        for (int i = Keys.Count - 1; i >= 0; i--){
            if(Keys[i].Key != key){
                continue;
            }
            if(!Keys[i].IsAttached){
                if(Keys[i].ReduceHealth(1)){
                    TroopsManager.S_SpawnedCharacters.Remove(Keys[i].Character);
                    Keys.RemoveAt(i);
                }
                return true;
            }
            if(AttachedKeys.Count > 0 && Keys[i] == AttachedKeys.Peek()){
                if(Keys[i].ReduceHealth(1)){
                    Dequeue();
                    TroopsManager.S_SpawnedCharacters.Remove(Keys[i].Character);
                    Keys.RemoveAt(i);
                }
                return true;
            }
        }
        

        return false;
    }

    public void ResetSequence(){
        AttachedKeys.Clear();
    }
}