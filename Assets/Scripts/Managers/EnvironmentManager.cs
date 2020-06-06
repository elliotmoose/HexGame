using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{

    public float currentTemperature {
        get {
            //compute temperature based on ratio and time of day
            float minTemp = 25;
            float maxTemp = 65;
            float targetCO2 = 30;
            float percentageTemp = (currentCO2-targetCO2)/(currentCO2 + currentO2 - targetCO2);
            return (maxTemp-minTemp)*(percentageTemp) + minTemp;
        }
    }

    public float currentCO2 = 70;
    public float currentO2 {
        get {
            return 100-currentCO2;
        }
    }

    public void ProduceO2(float amount)
    {
        currentCO2 -= amount;
    }

    void Start() 
    {

    }

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
