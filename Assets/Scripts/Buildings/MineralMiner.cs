using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralMiner : Building
{
    private float idealEnergyInput = 10;
    public float mineralMiningRate = 15;

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
            Player.GetInstance().TransactResource(ResourceIdentifiers.MINERALS, ScaledOutputByResource(ResourceIdentifiers.ENERGY, mineralMiningRate));
        }
    }
}
