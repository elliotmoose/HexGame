using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceConsumer : MonoBehaviour
{
    protected Dictionary<ResourceIdentifiers, float> resources = new Dictionary<ResourceIdentifiers, float>();
    protected GameObject resourceIndicatorSet = null;

    protected virtual void InitializeResourceNeeds() {}
    
    public void SetNeedsResource(ResourceIdentifiers resourceId) 
    {
        resources.Add(resourceId, 0);
        // Debug.Log($"{id} now needs {resourceId}");
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
        else 
        {
            // Debug.Log($"{id} does not need {resourceId}");
        }
    }

    private void CreateIndicatorSetIfNeeded() 
    {
        //create if needed   
        if(resourceIndicatorSet == null)
        {
            resourceIndicatorSet = GameObject.Instantiate(PrefabManager.GetInstance().indicatorSet, UIManager.WorldToUISpace(this.transform.position), Quaternion.identity, UIManager.GetCanvas().transform);            
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

    protected void UpdateIndicatorsPosition() 
    {
        resourceIndicatorSet.transform.position = UIManager.WorldToUISpace(this.transform.position);
    }
}