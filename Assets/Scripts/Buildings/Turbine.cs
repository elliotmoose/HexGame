using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbine : Generator
{
    public GameObject blades;
    float bladeRotationSpeed = 50;

    // Start is called before the first frame update
    void Start()
    {
        this.energyOutput = 2;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        blades.transform.Rotate(0,0,bladeRotationSpeed*Time.deltaTime);
    }
}
