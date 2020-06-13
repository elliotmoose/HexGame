using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilPump : Building
{
    private float oilAmount = 50;
    private float idealEnergyInput = 10;


    // Start is called before the first frame update
    protected override void InitializeResourceNeeds() 
    {
        SetNeedsResource(ResourceIdentifiers.ENERGY, idealEnergyInput);
        AddResourceIndicator(ResourceIdentifiers.ENERGY, "This building needs energy to work!");
    }

    public override void BuildingTick() 
    {
        if(HasResource(ResourceIdentifiers.ENERGY))
        {
            Player.GetInstance().TransactResource(ResourceIdentifiers.OIL, oilAmount);
        }
    }
}
