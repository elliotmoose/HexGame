
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningPlatform : HexPlatform
{
    public float mineralAmount = 15;
    public float mineralInterval = 1;
    private float _curMineralInterval = 0;

    // Start is called before the first frame update
    void Start()
    {
        
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
        Player.GetInstance().TransactMinerals(mineralAmount);
    }
}
