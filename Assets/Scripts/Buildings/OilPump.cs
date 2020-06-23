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
        MetaDataParameter energyInputParameter = metaData.GetParameterForKey("ENERGY_INPUT_IDEAL");
        idealEnergyInput = energyInputParameter.value;
        metaData.MapParameterForKey("OIL_OUTPUT_IDEAL", out idealOilOutput);
        SetNeedsResource(ResourceIdentifiers.ENERGY, energyInputParameter);
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
