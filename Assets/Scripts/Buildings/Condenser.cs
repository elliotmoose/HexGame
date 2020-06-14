using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condenser : Building
{
    float idealEnergyInput = 15;
    float idealWaterOutput = 10;
    float idealCoolFactor = 7;
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
        SetNeedsResource(ResourceIdentifiers.ENERGY, idealEnergyInput);
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
        return $"Energy Input: {GetResource(ResourceIdentifiers.ENERGY)}/{GetResourceIdeal(ResourceIdentifiers.ENERGY)}\nWater Output: {waterOutput}\nCool Output: {coolFactor}";
    }
}
