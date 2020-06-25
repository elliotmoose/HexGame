using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourceConsumer : MonoBehaviour
{
    protected Dictionary<ResourceIdentifiers, ResourceMetaData> resources = new Dictionary<ResourceIdentifiers, ResourceMetaData>();


    protected GameObject resourceIndicatorSet = null;
    public int resourceCalculationOrder = 0;

    protected virtual void InitializeResourceNeeds() {}
    
    public void SetNeedsResource(ResourceIdentifiers resourceId, MetaDataParameter parameter, bool startMax=false, bool persist=false) 
    {
        resources.Add(resourceId, new ResourceMetaData(parameter, startMax ? parameter.value : 0, persist));
    }

    public void SetResourceIdeal(ResourceIdentifiers resourceId, float ideal) 
    {
        resources[resourceId].ideal = ideal;
    }

    public bool NeedsResource(ResourceIdentifiers resourceId) 
    {
        //if a resource has the key it needs the resource        
        return resources.ContainsKey(resourceId) && GetResource(resourceId).active;
    }

    protected bool HasResource(ResourceIdentifiers resourceId) 
    {
        return GetResource(resourceId).value != 0;
    }
    
    public ResourceMetaData GetResource(ResourceIdentifiers resourceId) 
    {
        ResourceMetaData resource = null;
        resources.TryGetValue(resourceId, out resource);
        if(resource == null)
        {
            return new ResourceMetaData();
        }
        return resource;
    }


    protected float ScaledOutputByResource(ResourceIdentifiers resourceId, float output)
    {
        ResourceMetaData resource = GetResource(resourceId);
        if(resource == null)
        {
            Debug.Log($"Could not scale resource {resourceId} as it is not needed on this consumer");
            return 0;
        }
        return output * resource.fulfillFactor;
    }
    
    public float ExpendAllResource(ResourceIdentifiers resourceId) 
    {
        ResourceMetaData resource = GetResource(resourceId);
        if(resource == null)
        {
            Debug.Log($"Could not expend resource {resourceId} as it is not needed on this consumer");
            return 0;
        }
        
        float oldValue = resource.value;
        resource.value = 0;
        return oldValue;
    }


    public void ReceiveResource(ResourceIdentifiers resourceId, float amount) 
    {
        if(NeedsResource(resourceId))
        {
            ResourceMetaData resource = GetResource(resourceId);
            resource.value += amount;
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
            //don't reset persist resources
            if(GetResource(resourceId).persist)
            {
                continue;
            }
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
            SetResourceIndicator(resourceId, !HasResource(resourceId) && NeedsResource(resourceId));
        }
    }
}