using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Building
{
    public float energyOutput = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        foreach (Building neighbour in neighbourBuildings)
        {
            if(neighbour.NeedsResource(ResourceIdentifiers.ENERGY))
            {
                neighbour.ReceiveResource(ResourceIdentifiers.ENERGY, energyOutput);
            }
        }

        foreach (HexPlatform neighbour in neighbourPlatforms)
        {
            if(neighbour.NeedsResource(ResourceIdentifiers.ENERGY))
            {
                neighbour.ReceiveResource(ResourceIdentifiers.ENERGY, energyOutput);
            }
        }
    
    }

    // public override void OnSystemUpdateBuilding()
    public override void BuildingTick()
    {
        // List<Building> neighbours = this.neighbourBuildings;
        
    }
}
