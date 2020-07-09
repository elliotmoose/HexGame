using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="TileMetaData", menuName ="Scriptables/Tile Meta Data",order=2)]
public class TileMetaData : ScriptableObject
{
    public Identifiers id;
    public string title;
    public GameObject prefab;
    public Color color;
    public float threshold;
}