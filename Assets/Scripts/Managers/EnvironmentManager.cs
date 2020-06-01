using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{

    public float currentTemperature {
        get {
            //compute temperature based on ratio 
            float minTemp = 25;
            float maxTemp = 80;
            float percentageTemp = currentCO2/(currentCO2 + currentO2);
            return (maxTemp-minTemp)*(percentageTemp) + minTemp;
        }
    }
    public float currentCO2 = 70;
    public float currentO2 = 30;

    // Update is called once per frame
    void Update()
    {

    }

    private static EnvironmentManager _singleton;

    EnvironmentManager() {
        _singleton = this;
    }

    public static EnvironmentManager GetInstance() 
    {
        return _singleton;
    }
}
