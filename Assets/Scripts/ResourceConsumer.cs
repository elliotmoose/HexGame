using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceConsumer : MonoBehaviour
{
    protected Dictionary<ResourceIdentifiers, float> resources = new Dictionary<ResourceIdentifiers, float>();

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
}