using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condenser : Building
{
    float waterOutput = 1;
    // Start is called before the first frame update
    void Start()
    {
    }
    
    protected override void InitializeResourceNeeds()
    {
        SetNeedsResource(ResourceIdentifiers.ENERGY);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public override void OnSystemUpdateBuilding()
    public override void Tick()
    {
        if(!HasResource(ResourceIdentifiers.ENERGY))
        {
            // Debug.Log("Condenser does not have energy!");
            return;
        }
        // List<Building> neighbours = this.neighbourBuildings;
        foreach (Building neighbour in neighbourBuildings)
        {
            if(neighbour.NeedsResource(ResourceIdentifiers.WATER))
            {
                neighbour.ReceiveResource(ResourceIdentifiers.WATER, waterOutput);
            }
        }
    }
}
