﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Building
{
    public float energyOutput = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public override void OnSystemUpdateBuilding()
    public override void Tick()
    {
                // List<Building> neighbours = this.neighbourBuildings;
        foreach (Building neighbour in neighbourBuildings)
        {
            if(neighbour.NeedsResource(ResourceIdentifiers.ENERGY))
            {
                neighbour.ReceiveResource(ResourceIdentifiers.ENERGY, energyOutput);
            }
        }
    }
}