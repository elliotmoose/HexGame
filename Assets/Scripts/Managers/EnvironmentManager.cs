using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public const float DAY_IN_SECONDS = 24*60*60;
    [System.NonSerialized]
    public float timeOfDay = 400 * 0.25f * DAY_IN_SECONDS;
    [System.NonSerialized]
    public float dayLength = 400 * DAY_IN_SECONDS; //a day on this planet is 400 earth days: 400 * 24 * 60 * 60 = 34560000
    public float gameTimeDayLength = 60; //a day in the game should last about 25mins
    
    public float timeOfDayFraction {
        get {
            return (timeOfDay % dayLength)/dayLength;
        }
    }

    public bool isDay {
        get {
            return (timeOfDayFraction > 0.25f && timeOfDayFraction < 0.75f);
        }
    }

    public float daylight {
        get {
            if(!isDay) 
            {
                return 0;
            } 

            //peak is at 0.5
            //y = -(4x-2)^2 + 1
            return -Mathf.Pow((4*timeOfDayFraction - 2), 2) + 1;
        }
    }

    public float currentCO2 = 70;
    public float currentO2 {
        get {
            return 100-currentCO2;
        }
    }

    private float targetCO2 = 30;
    private float co2Factor {
        get {            
            //100% CO2 will give 1
            //target CO2 will give 0
            return Mathf.Clamp((currentCO2-targetCO2)/(100 - targetCO2), 0, 1);
        }
    }


    #region Seasons
    private int seasonLengthInDays = 7;//7 days per season
    private float gameTimeSeasonLength {
        get {
            return seasonLengthInDays * gameTimeDayLength;
        }
    }
    private float timeInSeasons = 0;
    private float timeInSeasonsFraction {
        get {
            float totalSeasonsLength = gameTimeSeasonLength * 4;
            return (timeInSeasons % totalSeasonsLength)/totalSeasonsLength;
        }
    }
    private string seasonName {
        get {
            return new string[]{"Spring", "Summer", "Autumn", "Winter"}[(int)(timeInSeasonsFraction * 4)];
        }
    }

    #endregion

    #region temperature
    private float seasonTemp {
        get {            
            float maxSeasonalTemp = 5;
            return Mathf.Sin(2*(timeInSeasonsFraction - 0.125f)*Mathf.PI) * maxSeasonalTemp;
        }
    }

    private float maxTemp {
        get {
            return Mathf.Lerp(35, 65, co2Factor);
        }
    }
    
    private float minTemp {
        get {
            return Mathf.Lerp(25, 35, co2Factor);
        }
    }

    public float currentTemperature {
        get {
            //compute temperature based on ratio and time of day
            float timeOfDaySinFactor = (-Mathf.Sin(2 * Mathf.PI * (timeOfDayFraction + 0.25f)) + 1)/2;
            return Mathf.Lerp(minTemp, maxTemp, timeOfDaySinFactor) + seasonTemp;
        }
    }

    #endregion

    public void ProduceO2(float amount)
    {
        currentCO2 -= amount;
    }

    void Start() 
    {
        StartCoroutine(EnvironmentTick());
    }

    // Update is called once per frame
    void Update()
    {
        timeOfDay += (Time.deltaTime * dayLength/gameTimeDayLength);
        timeInSeasons += Time.deltaTime;
    }

    IEnumerator EnvironmentTick() 
    {
        while(true)
        {
            //PERFORMANCE: might not want to recalculate every frame 
            PlatformManager.GetInstance().RecalculateResources();
            yield return null;
        }
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
