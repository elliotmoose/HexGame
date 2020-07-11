using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ResourceEvent(ResourceIdentifiers resource, float currentAmount);

public class Player : MonoBehaviour
{
    private Dictionary<ResourceIdentifiers, float> resources = new Dictionary<ResourceIdentifiers, float>();

    public ResourceEvent OnResourceFinished;
    public ResourceEvent OnResourceNonZero;

    // Start is called before the first frame update
    void Awake()
    {
        resources.Add(ResourceIdentifiers.IRON, 1000);
        resources.Add(ResourceIdentifiers.COPPER, 0);
        resources.Add(ResourceIdentifiers.OIL, 1000);
        resources.Add(ResourceIdentifiers.SEED, 1);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanAfford(List<Cost> costs) 
    {
        foreach(Cost cost in costs)   
        {
            if(cost.value > GetResource(cost.resourceId))
            {
                Debug.Log($"Insufficient resource: {cost.resourceId}");
                return false;
            }
        }

        return true;
    }

    public void TransactCosts(List<Cost> costs) 
    {
        foreach(Cost cost in costs)   
        {
            TransactResource(cost.resourceId, -cost.value);
        }
    }

    public void TransactResource(ResourceIdentifiers resourceId, float amount) 
    {
        if(!resources.ContainsKey(resourceId))
        {
            Debug.LogError($"PLayer does not have resource: {resourceId}");
            return;
        }
        float oldResourceAmount = resources[resourceId];
        resources[resourceId] += amount;
        float newResourceAmount = resources[resourceId];
        if(oldResourceAmount > 0 && newResourceAmount < 0)
        {
            if(OnResourceNonZero != null)
            {
                OnResourceFinished(resourceId, newResourceAmount);
            }
        }
        
        if(oldResourceAmount <= 0 && newResourceAmount > 0)
        {
            if(OnResourceNonZero != null)
            {
                OnResourceNonZero(resourceId, newResourceAmount);
            }
        }
    }


    public float GetResource(ResourceIdentifiers resourceId) 
    {
        return resources[resourceId];
    }

    public bool HasResource(ResourceIdentifiers resourceId) 
    {
        return GetResource(resourceId) > 0;
    }

    private static Player _singleton;

    Player() {
        _singleton = this;
    }

    public static Player GetInstance() 
    {
        return _singleton;
    }
}
