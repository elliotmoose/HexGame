using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, ResourceConsumer
{
    public Identifiers id = Identifiers.NULL;
    public Vector2Int coordinate = Vector2Int.zero;
    protected Dictionary<ResourceIdentifiers, float> resources = new Dictionary<ResourceIdentifiers, float>();

    protected List<Building> neighbourBuildings {
        get {
            HexPlatform buildingPlatform = PlatformManager.GetInstance().PlatformAtCoordinate(this.coordinate);
            List<HexPlatform> platforms = PlatformManager.GetInstance().NeighboursOfPlatform(buildingPlatform);
            List<Building> neighbours = new List<Building>();

            foreach (HexPlatform platformNeighbour in platforms)
            {
                if(platformNeighbour.building)
                {
                    neighbours.Add(platformNeighbour.building);
                }
            }

            return neighbours;
        }
    }

    public void Initialize(Identifiers id, Vector2Int coord) 
    {
        this.id = id;
        coordinate = coord;
        InitializeResourceNeeds();
    }

    protected virtual void InitializeResourceNeeds() {}
    
    public void SetNeedsResource(ResourceIdentifiers resourceId) 
    {
        resources.Add(resourceId, 0);
        // Debug.Log($"{id} now needs {resourceId}");
    }
    
    public bool NeedsResource(ResourceIdentifiers resourceId) 
    {
        //if a resource has the key it needs the resource
        return resources.ContainsKey(resourceId);
    }

    protected bool HasResource(ResourceIdentifiers resourceId) 
    {
        float resource = 0;
        resources.TryGetValue(resourceId, out resource);
        return resource != 0;
    }


    public void ReceiveResource(ResourceIdentifiers resourceId, float amount) 
    {
        if(NeedsResource(resourceId))
        {
            resources[resourceId] += amount;
        }
        else 
        {
            // Debug.Log($"{id} does not need {resourceId}");
        }
    }

    public virtual void Tick() {}

    public virtual void OnSystemUpdateBuilding() {}
}
