using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : ResourceConsumer
{
    public ObjectMetaData metaData;
    public Vector2Int coordinate = Vector2Int.zero;

    protected List<HexPlatform> neighbourPlatforms {
        get {
            HexPlatform buildingPlatform = PlatformManager.GetInstance().PlatformAtCoordinate(this.coordinate);
            return PlatformManager.GetInstance().NeighboursOfPlatform(buildingPlatform);
        }
    }

    protected List<Building> neighbourBuildings {
        get {
            HexPlatform buildingPlatform = PlatformManager.GetInstance().PlatformAtCoordinate(this.coordinate);
            List<HexPlatform> platforms = PlatformManager.GetInstance().NeighboursOfPlatform(buildingPlatform);
            List<Building> neighbours = new List<Building>();

            foreach (HexPlatform platformNeighbour in platforms)
            {
                if(platformNeighbour.building != null)
                {
                    neighbours.Add(platformNeighbour.building);
                }
            }

            return neighbours;
        }
    }

    protected List<Building> GetNeighbourBuildingsWithAxis(int axis)
    {
        HexPlatform buildingPlatform = PlatformManager.GetInstance().PlatformAtCoordinate(this.coordinate);
        List<HexPlatform> platforms = PlatformManager.GetInstance().NeighboursOfPlatformWithAxis(buildingPlatform, axis);
        List<Building> neighbours = new List<Building>();

        foreach (HexPlatform platformNeighbour in platforms)
        {
            if(platformNeighbour.building != null)
            {
                neighbours.Add(platformNeighbour.building);
            }
        }

        return neighbours;
    }

    public void Initialize(ObjectMetaData metaData, Vector2Int coord) 
    {
        this.metaData = metaData;
        coordinate = coord;
        resourceCalculationOrder = metaData.resourceRecalculationOrder;
        InitializeResourceNeeds();
    }

    public virtual void UpgradeHandoverFrom(Building oldBuilding)
    {
        Debug.Log("No handover!");
    }

    protected virtual void Update() 
    {
        
    }

    public virtual string GetActionText() {return "";}
    public virtual void Action() {}
    
    public virtual List<Metric> GetMetrics() {
        List<Metric> metrics = new List<Metric>();

        foreach(ResourceMetaData resource in resources.Values)
        {
            if(resource.displayType != MetricDisplayType.None)
            {
                Metric metric = new Metric($"{resource.readableKey}:", resource.value, resource.ideal, resource.displayType);
                metrics.Add(metric);    
            }
        }

        return metrics;
    }
    

    public virtual void BuildingTick() {}

    public virtual string GetDescription() {return"";}
}
