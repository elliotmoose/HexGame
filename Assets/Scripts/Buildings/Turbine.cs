using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbine : Building
{
    public GameObject blades;
    public float _idealEnergyOutput;
    float bladeRotationSpeed = 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void InitializeResourceNeeds()
    {        
        metaData.MapParameterForKey("ENERGY_OUTPUT_IDEAL", out _idealEnergyOutput);
    }

    // Update is called once per frame
    protected override void Update()
    {
        blades.transform.Rotate(0,0,bladeRotationSpeed*Time.deltaTime);
    }

    public override void RecalculateResources()
    {
        foreach (Building neighbour in neighbourBuildings)
        {
            if(neighbour.NeedsResource(ResourceIdentifiers.ENERGY))
            {
                neighbour.ReceiveResource(ResourceIdentifiers.ENERGY, _idealEnergyOutput);
            }
        }

        foreach (HexPlatform neighbour in neighbourPlatforms)
        {
            if(neighbour.NeedsResource(ResourceIdentifiers.ENERGY))
            {
                neighbour.ReceiveResource(ResourceIdentifiers.ENERGY, _idealEnergyOutput);
            }
        }
    }
}
