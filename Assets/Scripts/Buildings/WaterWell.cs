using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWell : Building
{
    float idealWaterOutput;
    float waterCapacity;
    int split = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    protected override void InitializeResourceNeeds()
    {
        metaData.MapParameterForKey("WATER_OUTPUT_IDEAL", out idealWaterOutput);
        metaData.MapParameterForKey("WATER_CAPACITY", out waterCapacity);

    }

    // Update is called once per frame
    protected override void Update()
    {
        if(waterCapacity > 0)
        {
            waterCapacity -= idealWaterOutput * Time.deltaTime * split;
            if(waterCapacity <= 0)
            {
                PlatformManager.GetInstance().RecalculateResources();
            }
        }
    }

    public override void RecalculateResources()
    {        
        if(waterCapacity <= 0)
        {
            return;
        }

        split = 0;
        foreach (Building neighbour in neighbourBuildings)
        {
            if(neighbour.NeedsResource(ResourceIdentifiers.WATER))
            {
                split += 1;
                neighbour.ReceiveResource(ResourceIdentifiers.WATER, idealWaterOutput);                
            }
        }

        base.RecalculateResources();
    }

    public override string GetDescription()
    {
        return $"nWater Remainding: {waterCapacity}\n";
    }
}
