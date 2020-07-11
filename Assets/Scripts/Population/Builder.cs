using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public Vector2Int coordinate = Vector2Int.zero;
    private Vector3 target = Vector3.zero;

    private float moveTime = 0.5f;
    private float _movementProgress = 1f;

    private List<Task> tasks = new List<Task>();
    bool isIdle = true;

    public void MoveTo(Vector2Int targetCoord)
    {
        _movementProgress = 0;
        Vector3 targetPos = Hexagon.PositionForCoordinate(targetCoord);
        float height = HexMapManager.GetInstance().HeightMap(targetCoord)/2 + this.transform.localScale.y;
        float radialOffsetDistance = 0.6f;
        // float randomAngle = Random.Range(0,359);
        // Debug.Log(randomAngle);
        // Vector3 randomOffset = Quaternion.Euler(0, randomAngle, 0) * new Vector3(1,0,0) * radialOffsetDistance;
        
        Vector3 offCenterOffset = -Vector3.Scale((targetPos - this.transform.position).normalized, new Vector3(1,0,1)).normalized * radialOffsetDistance;
        target = new Vector3(targetPos.x, height ,targetPos.z) + offCenterOffset;
    }

    public void Build(BuildingMetaData building, Vector2Int coord)
    {
        isIdle = false;   
        //charge costs
        Player.GetInstance().TransactCosts(building.costs);            
        
        //add to queue                
        tasks.Add(new Task(building, coord));
        Builder.GetInstance().MoveTo(coord);
    }

    void OnArriveAtTile() 
    {
        if(tasks.Count > 0)
        {
            Task task = tasks[0];
            GameObject platform = BuildingsManager.GetInstance().Build(task.data, task.coordinate);
            tasks.RemoveAt(0);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(_movementProgress < 1)
        {
            float oldProgress = _movementProgress;
            _movementProgress += Time.deltaTime/moveTime;
            if(oldProgress < 1 && _movementProgress > 1)
            {
                OnArriveAtTile();
            }
            
            this.transform.position = Vector3.Lerp(this.transform.position, target, _movementProgress);
        }
    }

    private static Builder _singleton;

    Builder() {
        _singleton = this;
    }

    public static Builder GetInstance() 
    {
        return _singleton;
    }
}


class Task 
{
    public BuildingMetaData data;
    public Vector2Int coordinate;

    public Task(BuildingMetaData data, Vector2Int coord) 
    {
        this.data = data;
        this.coordinate = coord;
    }
}