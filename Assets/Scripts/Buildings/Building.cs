using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : ResourceConsumer
{
    public BuildingMetaData metaData;
    public Vector2Int coordinate = Vector2Int.zero;

    protected List<Building> neighbourBuildings {
        get {
            return BuildingsManager.GetInstance().NeighboursOf(this);
        }
    }

    protected List<Building> GetNeighbourBuildingsWithAxis(int axis)
    {
        return BuildingsManager.GetInstance().NeighboursOf(this, axis);
    }

    public void Initialize(BuildingMetaData metaData, Vector2Int coord) 
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
    public virtual bool CanDestroy() {return true;}
    
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
