using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Building
{
    public GameObject treeObject;
    private float _maxAge = 15;
    private float _curAge = 0;
    private float health = 0;

    void Awake()
    {
        treeObject.transform.localScale = Vector3.zero;
    }

    protected override void InitializeResourceNeeds()
    {
        SetNeedsResource(ResourceIdentifiers.WATER);
        SetNeedsResource(ResourceIdentifiers.LIGHT);
    }

    void Update() 
    {
        if(_curAge < _maxAge)
        {
            if(!HasResource(ResourceIdentifiers.WATER) || !HasResource(ResourceIdentifiers.LIGHT)) 
            {
                return;
            }
            _curAge += Time.deltaTime;
            float ageFactor = _curAge/_maxAge;
            treeObject.transform.localScale = new Vector3(ageFactor, ageFactor, ageFactor);

            float maxAngle = 40;
            treeObject.transform.rotation = Quaternion.Euler(0, ageFactor * maxAngle, 0);
        }
    }

    /// <summary>
    /// Checks if the platform this tree is on has the requirements it needs to grow
    /// </summary>
    public override void OnSystemUpdateBuilding()
    {
        //day time
        ReceiveResource(ResourceIdentifiers.LIGHT, 1);
    }
}