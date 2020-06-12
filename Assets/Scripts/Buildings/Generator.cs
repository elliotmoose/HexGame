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

    protected override void InitializeResourceNeeds()
    {
        SetNeedsResource(ResourceIdentifiers.OIL);
        AddResourceIndicator(ResourceIdentifiers.OIL, "Needs oil to generate energy!");
    }

    // Update is called once per frame
    protected override void Update()
    {
        float oilConsumptionRate = 100;
        Player player = Player.GetInstance();
        float consumedThisFrame = Mathf.Min(oilConsumptionRate * Time.deltaTime, player.oil);
        player.TransactResource(ResourceIdentifiers.OIL, -consumedThisFrame);
        ReceiveResource(ResourceIdentifiers.OIL, consumedThisFrame);
        base.Update(); //update ui       

        //if no oil consumed, no energy produced
        ExpendAllResource(ResourceIdentifiers.OIL);
        if(consumedThisFrame == 0)
        {
            Debug.Log(GetResource(ResourceIdentifiers.OIL));
            return;
        }
        

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
