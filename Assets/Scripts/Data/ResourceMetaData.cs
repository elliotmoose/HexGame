using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMetaData 
{
    public string key = "NULL";
    public string readableKey = "NULL";
    
    public MetricDisplayType displayType = MetricDisplayType.None;
    public bool active = true;
    public bool persist = false; //value persists through reset
    public float value = 0;
    public float ideal = 0;

    public float fulfillFactor {
        get {
            if(ideal == 0)
            {
                Debug.Log("No ideal");
                return 1;
            }
            return value/ideal;
        }
    }

    // public ResourceMetaData(float value, float ideal, string key="NULL", string readableKey="NULL") 
    // {
    //     active = true;
    //     this.value = value;
    //     this.ideal = ideal;
    //     this.key = key;
    //     this.readableKey = readableKey;
    // }

    public ResourceMetaData() {}

    public  ResourceMetaData(MetaDataParameter parameter, float initialValue=0, bool persist=false)
    {
        active = true;
        this.value = initialValue;
        this.ideal = parameter.value;
        this.key = parameter.key;
        this.readableKey = parameter.readableKey;
        this.displayType = parameter.displayType;
        this.persist = persist;
    }
}