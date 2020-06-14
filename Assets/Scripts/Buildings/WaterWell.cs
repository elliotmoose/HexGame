using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWell : Building
{
    float idealWaterOutput = 10;
    float waterCapacity = 5000;
    int split = 0;
    // Start is called before the first frame update
    void Start()
    {

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
