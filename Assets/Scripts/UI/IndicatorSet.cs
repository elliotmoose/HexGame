using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IndicatorSet : MonoBehaviour
{
    public GameObject indicatorPrefab;
    public Dictionary<ResourceIdentifiers, string> displayedResources = new Dictionary<ResourceIdentifiers, string>();
    private Dictionary<ResourceIdentifiers, GameObject> indicators = new Dictionary<ResourceIdentifiers, GameObject>();

    // Start is called before the first frame update
    public void AddIndicator(ResourceIdentifiers resourceID, string message)
    {
        displayedResources.Add(resourceID, message);
        UpdateIndicators();
    }

    private void UpdateIndicators() 
    {        
        foreach (GameObject indicator in indicators.Values)
        {
            GameObject.Destroy(indicator);
        }

        indicators.Clear();

        foreach(ResourceIdentifiers resourceId in displayedResources.Keys)
        {
            GameObject indicatorObject = GameObject.Instantiate(indicatorPrefab, Vector3.zero, Quaternion.identity, this.transform.GetChild(0));
            indicatorObject.transform.localPosition = Vector3.zero;
            indicatorObject.GetComponent<Indicator>().Initialize(resourceId, displayedResources[resourceId]);
            indicators.Add(resourceId, indicatorObject);
        }
    }

    public void SetShowIndicator(ResourceIdentifiers resourceId, bool active) 
    {
        if(!indicators.ContainsKey(resourceId))
        {
            Debug.LogWarning($"This indicator set does not have an indicator for the resource Id: {resourceId}");
            return;
        }

        indicators[resourceId].SetActive(active);
    }
}
