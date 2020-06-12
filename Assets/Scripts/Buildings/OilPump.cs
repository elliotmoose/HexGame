using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilPump : Building
{

    public float oilAmount = 15;
    public float oilInterval = 1;
    private float _curOilInterval = 0;

    // Start is called before the first frame update
    protected override void InitializeResourceNeeds() 
    {
        SetNeedsResource(ResourceIdentifiers.ENERGY);
        AddResourceIndicator(ResourceIdentifiers.ENERGY, "This building needs energy to work!");
    }

    public override void BuildingTick() 
    {
        if(HasResource(ResourceIdentifiers.ENERGY))
        {
            Player.GetInstance().TransactResource(ResourceIdentifiers.OIL, oilAmount);
        }
    }

    protected override void Update()
    {
        base.Update();
        _curOilInterval += Time.deltaTime;
        if(_curOilInterval >= oilInterval)
        {
            Trigger();
            _curOilInterval = 0;
        }
    }

    void Trigger() 
    {
    }
}
