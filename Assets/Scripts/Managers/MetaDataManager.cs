using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaDataManager : MonoBehaviour
{    
    public List<ObjectMetaData> buildingMetaData = new List<ObjectMetaData>();
    public List<ObjectMetaData> platformMetaData = new List<ObjectMetaData>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static ObjectMetaData MetaDataForId(Identifiers id)
    {
        foreach(ObjectMetaData item in _singleton.buildingMetaData) 
        {
            if(item.id == id)
            {
                return item;
            }
        }
        
        foreach(ObjectMetaData item in _singleton.platformMetaData) 
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
