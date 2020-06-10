using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="ShopItem", menuName ="Scriptables/Shop Item Scriptable",order=1)]
public class ShopItemScriptable : ScriptableObject
{
    public Identifiers id;
    public string title;
    public string description;
    public float price;
    public GameObject prefab;
    public GameObject displayPrefab;
}
