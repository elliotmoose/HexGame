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
    public List<ObjectMetaData> availableShopItems = new List<ObjectMetaData>();
}


public enum ShopItemType 
{
    BUILDING,
    PLATFORM
}