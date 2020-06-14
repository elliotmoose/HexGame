using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ResourceMetaData 
{
    bool active;
    float value;
    float ideal;
    
    ResourceMetaData(float value, float ideal) 
    {
        active = true;
        this.value = value;
        this.ideal = ideal;
    }
}

public class ResourceConsumer : MonoBehaviour
{
    protected Dictionary<ResourceIdentifiers, float> resources = new Dictionary<ResourceIdentifiers, float>();
    protected Dictionary<ResourceIdentifiers, float> idealResources = new Dictionary<ResourceIdentifiers, float>();
    protected GameObject resourceIndicatorSet = null;
    public int resourceCalculationOrder = 0;

    protected virtual void InitializeResourceNeeds() {}
    
    public void SetNeedsResource(ResourceIdentifiers resourceId, float ideal) 
    {
        resources.Add(resourceId, 0);
        idealResources.Add(resourceId, ideal);
    }

    public void SetResourceIdeal(ResourceIdentifiers resourceId, float ideal) 
    {
        idealResources[resourceId] = ideal;
    }

    public bool NeedsResource(ResourceIdentifiers resourceId) 
    {
        //if a resource has the key it needs the resource
        return resources.ContainsKey(resourceId);
    }

    protected bool HasResource(ResourceIdentifiers resourceId) 
    {
        float resource = 0;
        resources.TryGetValue(resourceId, out resource);
        return resource != 0;
    }
    
    protected float GetResource(ResourceIdentifiers resourceId) 
    {
        float resource = 0;
        resources.TryGetValue(resourceId, out resource);
        return resource;
    }
    
    protected float GetResourceIdeal(ResourceIdentifiers resourceId) 
    {
        float resourceIdeal = 0;
        idealResources.TryGetValue(resourceId, out resourceIdeal);
        return resourceIdeal;
    }
    
    protected float GetResourceFulfillFactor(ResourceIdentifiers resourceId) 
    {
        if(!resources.ContainsKey(resourceId))
        {
            Debug.LogError($"This consumer does not have resource: {resourceId}");
            return 0;
        }
        float resource = 0;
        float resourceIdeal = 0;
        resources.TryGetValue(resourceId, out resource);
        idealResources.TryGetValue(resourceId, out resourceIdeal);

        return resource/resourceIdeal;
    }

    protected float ScaledOutputByResource(ResourceIdentifiers resourceId, float output)
    {
        return output * GetResourceFulfillFactor(resourceId);
    }
    
    protected float ExpendAllResource(ResourceIdentifiers resourceId) 
    {
        float resource = 0;
        resources.TryGetValue(resourceId, out resource);
        resources[resourceId] = 0;
        return resource;
    }


    public void ReceiveResource(ResourceIdentifiers resourceId, float amount) 
    {
        if(NeedsResource(resourceId))
        {
            resources[resourceId] += amount;
        }
        // else 
        // {
        //     Debug.Log($"{id} does not need {resourceId}");
        // }
    }

    public virtual void ResetResources() {
        List<ResourceIdentifiers> resourceIds = new List<ResourceIdentifiers>(resources.Keys);
        foreach(ResourceIdentifiers resourceId in resourceIds) 
        {
            ExpendAllResource(resourceId);
        }
    }

    public virtual void RecalculateResources() {
        UpdateResourceIndicators();
    }

    private void CreateIndicatorSetIfNeeded() 
    {
        //create if needed   
        if(resourceIndicatorSet == null)
        {
            resourceIndicatorSet = GameObject.Instantiate(PrefabManager.GetInstance().indicatorSet, UIManager.WorldToUISpace(this.transform.position), Quaternion.identity, UIManager.GetCanvas().transform);
            resourceIndicatorSet.GetComponent<IndicatorSet>().owner = this.gameObject;            
        }
    }
    
    public void AddResourceIndicator(ResourceIdentifiers resourceId, string message) 
    {
        CreateIndicatorSetIfNeeded();   
        resourceIndicatorSet.GetComponent<IndicatorSet>().AddIndicator(resourceId, message);
    }
    
    protected void SetResourceIndicator(ResourceIdentifiers resourceId, bool isActive) 
    {
        if(!resourceIndicatorSet)
        {
            return;
        }

        resourceIndicatorSet.GetComponent<IndicatorSet>().SetShowIndicator(resourceId, isActive);
    }

    protected void UpdateResourceIndicators() 
    {
        if(!resourceIndicatorSet)
        {
            return;
        }

        foreach(ResourceIdentifiers resourceId in resourceIndicatorSet.GetComponent<IndicatorSet>().displayedResources.Keys)
        {
            SetResourceIndicator(resourceId, !HasResource(resourceId));
        }
    }
}