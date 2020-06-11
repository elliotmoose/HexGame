using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbine : Generator
{
    public GameObject blades;
    float bladeRotationSpeed = 50;

    new float energyOutput = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        blades.transform.Rotate(0,0,bladeRotationSpeed*Time.deltaTime);
    }
}
