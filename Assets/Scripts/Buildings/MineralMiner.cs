using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralMiner : Building
{

    public float mineralAmount = 15;
    public float mineralInterval = 1;
    private float _curMineralInterval = 0;

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
            Player.GetInstance().TransactMinerals(mineralAmount);
        }
    }

    protected override void Update()
    {
        base.Update();
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
