using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarPanel : Building
{
    public float _idealLightInput;
    public float _idealEnergyOutput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void InitializeResourceNeeds()
    {        
        metaData.MapParameterForKey("LIGHT_INPUT_IDEAL", out _idealLightInput);
        metaData.MapParameterForKey("ENERGY_OUTPUT_IDEAL", out _idealEnergyOutput);
        SetNeedsResource(ResourceIdentifiers.LIGHT, _idealLightInput);
        AddResourceIndicator(ResourceIdentifiers.LIGHT, "Needs light to generate energy");
    }

    public override void RecalculateResources()
    {
        float energyOutput = ScaledOutputByResource(ResourceIdentifiers.LIGHT, _idealEnergyOutput);

        foreach (Building neighbour in neighbourBuildings)
        {
            if(neighbour.NeedsResource(ResourceIdentifiers.ENERGY))
            {
                neighbour.ReceiveResource(ResourceIdentifiers.ENERGY, energyOutput);
            }
        }

        base.RecalculateResources();
    }

    public override string GetDescription()
    {        
        ResourceMetaData light = GetResource(ResourceIdentifiers.LIGHT);
        return $"Light Input: {(int)light.value} ({(int)(light.fulfillFactor * 100)}%)\nEnergy Output: {ScaledOutputByResource(ResourceIdentifiers.LIGHT, _idealEnergyOutput)}";
    }
}
