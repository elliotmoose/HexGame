using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaDataManager : MonoBehaviour
{    
    public List<BuildingMetaData> buildingMetaData = new List<BuildingMetaData>();
    public List<BuildingMetaData> platformMetaData = new List<BuildingMetaData>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static BuildingMetaData GetMetaDataForId(Identifiers id)
    {
        foreach(BuildingMetaData item in _singleton.buildingMetaData) 
        {
            if(item.id == id)
            {
                return item;
            }
        }
        
        foreach(BuildingMetaData item in _singleton.platformMetaData) 
        {
            if(item.id == id)
            {
                return item;
            }
        }

        Debug.LogWarning($"Could not find meta data with id {id}");
        return null;
    }
    
    private static MetaDataManager _singleton;

    MetaDataManager() {
        _singleton = this;
    }

    public static MetaDataManager GetInstance() 
    {
        return _singleton;
    }
}
