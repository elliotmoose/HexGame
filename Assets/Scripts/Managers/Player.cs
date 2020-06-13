using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float minerals = 2000;
    public float oil = 500;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TransactResource(ResourceIdentifiers resourceId, float amount) 
    {
        
        switch (resourceId)
        {
            case ResourceIdentifiers.OIL:
                float oldOil = oil;                
                oil += amount;

                if((oldOil > 0 && oil <= 0) || oldOil <= 0 && oldOil > 0) 
                {
                    OilEmptyStatusChanged();
                }
                break;
            case ResourceIdentifiers.MINERALS:
                minerals += amount;
                break;
            default:
                Debug.LogError($"Player does not have the resource {resourceId}");
                return;
        }

        UIManager.GetInstance().UpdateUI();
    }

    private void OilEmptyStatusChanged() 
    {
        PlatformManager.GetInstance().RecalculateResources();
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
