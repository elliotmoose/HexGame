using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilPump : Building
{
    private float idealEnergyInput;
    private float idealOilOutput;


    // Start is called before the first frame update
    protected override void InitializeResourceNeeds() 
    {
        metaData.MapParameterForKey("ENERGY_INPUT_IDEAL", out idealEnergyInput);
        metaData.MapParameterForKey("OIL_OUTPUT_IDEAL", out idealOilOutput);
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
