using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbine : Building
{
    public GameObject blades;
    public float energyOutput = 2;
    float bladeRotationSpeed = 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void InitializeResourceNeeds()
    {
        SetNeedsResource(ResourceIdentifiers.OIL);
        AddResourceIndicator(ResourceIdentifiers.OIL, "Needs oil to generate energy!");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        foreach (Building neighbour in neighbourBuildings)
        {
            if(neighbour.NeedsResource(ResourceIdentifiers.ENERGY))
            {
                neighbour.ReceiveResource(ResourceIdentifiers.ENERGY, energyOutput);
            }
        }

        foreach (HexPlatform neighbour in neighbourPlatforms)
        {
            if(neighbour.NeedsResource(ResourceIdentifiers.ENERGY))
            {
                neighbour.ReceiveResource(ResourceIdentifiers.ENERGY, energyOutput);
            }
        }


        blades.transform.Rotate(0,0,bladeRotationSpeed*Time.deltaTime);
    }
}
