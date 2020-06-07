using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public const float DAY_IN_SECONDS = 24*60*60;
    [System.NonSerialized]
    public float timeOfDay = 0 * DAY_IN_SECONDS;
    [System.NonSerialized]
    public float dayLength = 400 * DAY_IN_SECONDS; //a day on this planet is 400 earth days: 400 * 24 * 60 * 60 = 34560000
    private float gameTimeDayLength = 60; //a day in the game should last about 25mins
    public float timeOfDayFraction {
        get {
            return (timeOfDay % dayLength)/dayLength;
        }
    }


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
        timeOfDay += (Time.deltaTime * dayLength/gameTimeDayLength);
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
