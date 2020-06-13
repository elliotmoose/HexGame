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
    public List<ObjectMetaData> availableShopItems = new List<ObjectMetaData>();

    //custom logic for shop items 
    public List<ObjectMetaData> GetContextualAvailableShopItems(Vector2Int coordinate)
    {
        GameObject feature = MapManager.GetInstance().FeatureAtCoordinate(coordinate);

        switch (this.id)
        {
            case Identifiers.PLACEHOLDER_PLATFORM:
                //feature means only can build dig site
                if(feature)
                {
                    return new List<ObjectMetaData>(new ObjectMetaData[]{MetaDataManager.GetMetaDataForId(Identifiers.DIG_SITE_PLATFORM)});
                }

                break;
            case Identifiers.DIG_SITE_PLATFORM:
                //if feature and dig site, only can build mineral harvester
                if(feature)
                {
                    return new List<ObjectMetaData>(new ObjectMetaData[]{MetaDataManager.GetMetaDataForId(Identifiers.MINERAL_HARVESTER_BUILDING)});
                }

                break;
            default:
                break;
        }
        return availableShopItems;
    }
}


public enum ShopItemType 
{
    BUILDING,
    PLATFORM
}