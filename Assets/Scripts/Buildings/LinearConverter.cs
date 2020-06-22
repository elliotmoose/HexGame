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
    private string resourceCapacityParameterKey = "RESOURCE_CAPACITY";
    
    private float _resourceCapacity;
    private float _resourceCapacityMax;
    private bool _resourceCapacityEnabled = false;

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
        if(metaData.HasParameterForKey(resourceCapacityParameterKey))
        {
            metaData.MapParameterForKey(resourceCapacityParameterKey, out _resourceCapacityMax);
            _resourceCapacity = _resourceCapacityMax;
            _resourceCapacityEnabled = true;
        }
        
        if(metaData.HasParameterForKey(inputIdealParameterKey))
        {
            metaData.MapParameterForKey(inputIdealParameterKey, out idealInput);
        }

        metaData.MapParameterForKey(outputIdealParameterKey, out idealOutput);
        MetaDataParameter inputParameter = metaData.GetParameterForKey(inputIdealParameterKey);
        SetNeedsResource(inputResource, idealInput, inputParameter.key, inputParameter.readableKey);
        if(inputResourceMessage != "")
        {
            AddResourceIndicator(inputResource, inputResourceMessage);
        }
    }

    protected override void Update()
    {
        base.Update();

        if(_resourceCapacityEnabled && _resourceCapacity > 0)
        {
            _resourceCapacity -= GetScaledSplitOutput() * split * Time.deltaTime;
            if(_resourceCapacity <= 0)
            {
                PlatformManager.GetInstance().RecalculateResources();
            }
        }
        
    }

    public override void RecalculateResources()
    {
        
        if(_resourceCapacityEnabled && _resourceCapacity <= 0)
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

        if(_resourceCapacityEnabled)
        {
            output += $"\nCapacity: {_resourceCapacity}";
        }
        
        return output;
    }

    public override List<Metric> GetMetrics() 
    {
        List<Metric> metrics = new List<Metric>();
            
        if(inputResource != ResourceIdentifiers.NULL)
        {
            ResourceMetaData inputResourceData = GetResource(inputResource);        
            Metric inputMetric = new Metric($"{inputResourceData.readableKey}:", inputResourceData.value, inputResourceData.ideal);
            metrics.Add(inputMetric);
        }

        if(outputResource != ResourceIdentifiers.NULL)
        {
            //wrong: output resource doesn't reside on object. need to find a way to get meta data from scriptable metadata parameter directly
            float output = ScaledOutputByResource(inputResource, idealOutput);                       
            string outputReadable = metaData.GetParameterForKey(outputIdealParameterKey).readableKey;
            Metric outputMetric = new Metric($"{outputReadable}:", output, idealOutput);
            
            metrics.Add(outputMetric);
        }

        if(_resourceCapacityEnabled)
        {
            Metric durabilityMetric = new Metric("Durability:", _resourceCapacity, _resourceCapacityMax);            
            metrics.Add(durabilityMetric);
        }

        return metrics;
    }
}
