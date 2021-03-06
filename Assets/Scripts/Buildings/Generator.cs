﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Building
{
    float oilInput;
    float idealEnergyOutput;
        
    int split = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void InitializeResourceNeeds()
    {
        metaData.MapParameterForKey("ENERGY_OUTPUT_IDEAL", out idealEnergyOutput);
        // metaData.MapParameterForKey("OIL_INPUT", out oilInput);
        MetaDataParameter oilInputParameter = metaData.GetParameterForKey("OIL_INPUT");
        oilInput = oilInputParameter.value;
        SetNeedsResource(ResourceIdentifiers.OIL, oilInputParameter);
        AddResourceIndicator(ResourceIdentifiers.OIL, "Needs oil to generate energy!");
    }

    // Update is called once per frame
    protected override void Update()
    {        
        ConsumeOil();
        base.Update(); //update ui   
        UpdateResourceIndicators();              
    }

    //complex case
    //oil is deducted every frame, but the oil resource on generators are not indicative of the amount of oil going in and out
    //oil resource only acts as a flip switch for generators
    //they are turned on/off when player has no oil
    public void ConsumeOil() 
    {
        if(split == 0)
        {
            return;
        }
        //draw oil from player source
        Player player = Player.GetInstance();
        float consumedThisFrame = Mathf.Min(oilInput * Time.deltaTime, player.GetResource(ResourceIdentifiers.OIL));
        player.TransactResource(ResourceIdentifiers.OIL, -consumedThisFrame);
    }

    public override void RecalculateResources()
    {
        if(!Player.GetInstance().HasResource(ResourceIdentifiers.OIL)) 
        {
            Debug.Log("No oil generator");
            return;
        }        

        split = 0;    
        foreach (Building neighbour in neighbourBuildings)
        {
            if(neighbour.NeedsResource(ResourceIdentifiers.ENERGY))
            {
                split += 1;
            }
        }
        
        //if someone needs the energy
        if(split != 0)
        {
            foreach (Building neighbour in neighbourBuildings)
            {                
                if(neighbour.NeedsResource(ResourceIdentifiers.ENERGY))
                {
                    float energyOutput = idealEnergyOutput/split;
                    neighbour.ReceiveResource(ResourceIdentifiers.ENERGY, energyOutput);                    
                }
            }
        }        

        base.RecalculateResources();
    }

    public override void BuildingTick()
    {
        
    }

    public override string GetDescription()
    {        
        if(split == 0) 
        {
            return "Not active (No surrounding building needs energy)";
        }
        return $"Energy Output: {idealEnergyOutput} (Shared amongst {split})";
    }
}
