using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralMiner : Building
{
    public float mineralOutputIdeal;

    // Start is called before the first frame update
    protected override void InitializeResourceNeeds() 
    {
        metaData.MapParameterForKey("MINERAL_OUTPUT_IDEAL", out mineralOutputIdeal);
        SetNeedsResource(ResourceIdentifiers.ENERGY, metaData.GetParameterForKey("ENERGY_INPUT_IDEAL"));
        AddResourceIndicator(ResourceIdentifiers.ENERGY, "This building needs energy to work!");

    }

    public override void BuildingTick() 
    {
        if(HasResource(ResourceIdentifiers.ENERGY))
        {
            float mineralOutput = ScaledOutputByResource(ResourceIdentifiers.ENERGY, mineralOutputIdeal);
            Player.GetInstance().TransactResource(ResourceIdentifiers.IRON, mineralOutput);
            UIManager.PopupText($"{mineralOutput}", this.gameObject);
        }
    }


    public override string GetDescription() 
    {
        ResourceMetaData resource = GetResource(ResourceIdentifiers.ENERGY);
        return $"Energy Input: {resource.value} ({Numbers.OneDP(resource.fulfillFactor*100)}%)";
    }
}
