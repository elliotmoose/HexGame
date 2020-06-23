using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condenser : Building
{
    float idealEnergyInput;
    float idealWaterOutput;
    float idealCoolFactor;
    float waterOutput {
        get {
            return ScaledOutputByResource(ResourceIdentifiers.ENERGY, idealWaterOutput);
        }
    }
    float coolFactor {
        get {
            return ScaledOutputByResource(ResourceIdentifiers.ENERGY, idealCoolFactor);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    
    protected override void InitializeResourceNeeds()
    {        
        metaData.MapParameterForKey("WATER_OUTPUT_IDEAL", out idealWaterOutput);
        metaData.MapParameterForKey("COOL_FACTOR_IDEAL", out idealCoolFactor);
        SetNeedsResource(ResourceIdentifiers.ENERGY, metaData.GetParameterForKey("ENERGY_INPUT_IDEAL"));
        AddResourceIndicator(ResourceIdentifiers.ENERGY, "Condenser needs energy to produce water!");
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    public override void RecalculateResources()
    {        
        if(!HasResource(ResourceIdentifiers.ENERGY))
        {
            return;
        }

        foreach (Building neighbour in neighbourBuildings)
        {
            if(neighbour.NeedsResource(ResourceIdentifiers.WATER))
            {
                neighbour.ReceiveResource(ResourceIdentifiers.WATER, waterOutput);
            }
            
            if(neighbour.NeedsResource(ResourceIdentifiers.COOL))
            {
                neighbour.ReceiveResource(ResourceIdentifiers.COOL, coolFactor);
            }
        }

        base.RecalculateResources();
    }

    public override string GetDescription()
    {
        return $"Energy Input: {GetResource(ResourceIdentifiers.ENERGY).value}/{GetResource(ResourceIdentifiers.ENERGY).ideal}\nWater Output: {waterOutput}\nCool Output: {coolFactor}";
    }
}
