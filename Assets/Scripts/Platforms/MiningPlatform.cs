
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningPlatform : HexPlatform
{
    public float mineralAmount = 15;
    public float mineralInterval = 1;
    private float _curMineralInterval = 0;

    // Start is called before the first frame update
    protected override void InitializeResourceNeeds() 
    {
        SetNeedsResource(ResourceIdentifiers.ENERGY);
    }

    protected override void PlatformTick() 
    {

        if(HasResource(ResourceIdentifiers.ENERGY))
        {
            Player.GetInstance().TransactResource(ResourceIdentifiers.MINERALS, mineralAmount);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _curMineralInterval += Time.deltaTime;
        if(_curMineralInterval >= mineralInterval)
        {
            Trigger();
            _curMineralInterval = 0;
        }
    }

    void Trigger() 
    {
    }
}
