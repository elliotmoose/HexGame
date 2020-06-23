using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MetaDataParameter 
{
    public string key;
    public MetricDisplayType displayType;
    public string readableKey;
    public float value;
}