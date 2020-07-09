using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="ObjectMetaData", menuName ="Scriptables/Object Meta Data",order=1)]
public class ObjectMetaData : ScriptableObject
{
    public Identifiers id;
    public string title;
    public string description;
    public float price;
    public GameObject prefab;
    public GameObject displayPrefab;
    public ShopItemType type;
    public int resourceRecalculationOrder;
    public ObjectMetaData upgrade;
    public List<TileMetaData> canBuildOn = new List<TileMetaData>();
    public List<MetaDataParameter> parameters = new List<MetaDataParameter>();

    public bool CanBuildOn(Identifiers tileId)
    {
        foreach(var tileData in canBuildOn)
        {
            if(tileData.id == tileId)
            {
                return true;
            }
        }
        
        return false;
    }

    public bool HasParameterForKey(string key) 
    {
        foreach(MetaDataParameter param in parameters)
        {
            if(param.key == key)
            {
                return true;
            }
        }
        
        return false;
    }

    public MetaDataParameter GetParameterForKey(string key) 
    {
        foreach(MetaDataParameter param in parameters)
        {
            if(param.key == key)
            {
                return param;
            }
        }
        
        return new MetaDataParameter();
    }

    public void MapParameterForKey(string key, out float value) 
    {
        foreach(MetaDataParameter param in parameters)
        {
            if(param.key == key)
            {
                value = param.value;
                return;
            }
        }
        
        Debug.LogWarning($"No parameter for {key} on {id}");
        value = 0;
    }
}

public enum ShopItemType 
{
    BUILDING,
    PLATFORM
}