using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilPump : Building
{
    private float idealEnergyInput = 10;
    private float idealOilOutput = 50;


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
            float oilOutput = ScaledOutputByResource(ResourceIdentifiers.ENERGY, idealOilOutput);
            Player.GetInstance().TransactResource(ResourceIdentifiers.OIL, oilOutput);
            UIManager.PopupText($"{oilOutput}", this.gameObject);
        }
    }
}
