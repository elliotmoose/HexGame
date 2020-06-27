using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearConverter : Building
{
    public ResourceIdentifiers inputResource = ResourceIdentifiers.NULL;
    public ResourceIdentifiers outputResource = ResourceIdentifiers.NULL;
    public string inputResourceMessage = "";
    private string inputIdealParameterKey = "INPUT_IDEAL";
    private string outputIdealParameterKey = "OUTPUT_IDEAL";
    private string durabilityParameterKey = "DURABILITY";

    private float idealInput;
    private float idealOutput;
    private int split = 0;

    public bool resourceOutputSplit = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void InitializeResourceNeeds()
    {        
        if(metaData.HasParameterForKey(durabilityParameterKey))
        {
            SetNeedsResource(ResourceIdentifiers.DURABILITY, metaData.GetParameterForKey(durabilityParameterKey), true, true);            
        }
        
        if(metaData.HasParameterForKey(inputIdealParameterKey))
        {
            SetNeedsResource(inputResource, metaData.GetParameterForKey(inputIdealParameterKey));
        }
        
        metaData.MapParameterForKey(outputIdealParameterKey, out idealOutput);

        if(inputResourceMessage != "")
        {
            AddResourceIndicator(inputResource, inputResourceMessage);
        }
    }

    protected override void Update()
    {
        base.Update();

        if(NeedsResource(ResourceIdentifiers.DURABILITY) && HasResource(ResourceIdentifiers.DURABILITY))
        {
            ReceiveResource(ResourceIdentifiers.DURABILITY, -GetScaledSplitOutput() * split * Time.deltaTime);
            if(!HasResource(ResourceIdentifiers.DURABILITY)) //ran out
            {
                PlatformManager.GetInstance().DestoryBuildingAtCoordinate(this.coordinate);
            }
        }
    }

    public override void RecalculateResources()
    {
        
        if(NeedsResource(ResourceIdentifiers.DURABILITY) && !HasResource(ResourceIdentifiers.DURABILITY))
        {
            return;
        }

        UpdateSplit();
        
        float output = GetScaledSplitOutput();

        foreach (Building neighbour in neighbourBuildings)
        {
            if(neighbour.NeedsResource(outputResource))
            {        
                neighbour.ReceiveResource(outputResource, output);
            }
        }

        base.RecalculateResources();
    }

    private void UpdateSplit()
    {
        if (resourceOutputSplit)
        {
            split = 0;
            foreach (Building neighbour in neighbourBuildings)
            {
                if (neighbour.NeedsResource(outputResource))
                {
                    split += 1;
                }
            }
        }
    }
    private float GetScaledSplitOutput() 
    {
        float scaledOutput = (inputResource != ResourceIdentifiers.NULL) ? ScaledOutputByResource(inputResource, idealOutput) : idealOutput;
        float splitOutput = scaledOutput / (resourceOutputSplit ? split : 1);
        return splitOutput;
    }

    public override void UpgradeHandoverFrom(Building oldBuilding)
    {
        if(NeedsResource(ResourceIdentifiers.DURABILITY))
        {
            ResourceMetaData oldResource = oldBuilding.GetResource(ResourceIdentifiers.DURABILITY);
            float previouslyConsumed = oldResource.ideal - oldResource.value;
            ReceiveResource(ResourceIdentifiers.DURABILITY, -previouslyConsumed);
        }
    }

    public override string GetDescription()
    {        
        
        ResourceMetaData inputResourceData = GetResource(inputResource);        
        ResourceMetaData outputResourceData = GetResource(outputResource);        
        string output = "";

        if(inputResource != ResourceIdentifiers.NULL)
        {
            output += $"{inputResource}: {(int)inputResourceData.value}";
            output += $"({(int)(inputResourceData.fulfillFactor * 100)}%)";
        }
        

        if(inputResource != ResourceIdentifiers.NULL)
        {
            output += $"\n{outputResource}: {ScaledOutputByResource(inputResource, idealOutput)}";
        }
        else 
        {
            output += $"\n{outputResource}: {idealOutput}";
        }

        if(resourceOutputSplit)
        {
            output += $" (Split amongst {split})";
        }

        // if(_resourceCapacityEnabled)
        // {
        //     output += $"\nCapacity: {_resourceCapacity}";
        // }
        
        return output;
    }

    public override List<Metric> GetMetrics() 
    {
        List<Metric> metrics = new List<Metric>();

        foreach(ResourceMetaData resource in resources.Values)
        {
            if(resource.displayType != MetricDisplayType.None)
            {
                Metric metric = new Metric($"{resource.readableKey}:", resource.value, resource.ideal, resource.displayType);
                metrics.Add(metric);    
            }
        }

        // if(_resourceCapacityEnabled)
        // {
        //     Metric durabilityMetric = new Metric("Durability:", _resourceCapacity, _resourceCapacityMax, MetricDisplayType.BarMetric);            
        //     metrics.Add(durabilityMetric);
        // }

        return metrics;
    }
}
