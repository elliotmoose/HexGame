using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbine : LinearConverter
{
    public GameObject blades;
    float bladeRotationSpeed = 50;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        blades.transform.Rotate(0,0,bladeRotationSpeed*Time.deltaTime);
    }
}
