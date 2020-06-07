using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsource : Building
{
    public new GameObject light;
    private float _turnonDuration = 1.6f;
    private float _intensity = 0;
    private bool _on = false;
    protected override void InitializeResourceNeeds()
    {
        SetNeedsResource(ResourceIdentifiers.ENERGY);
    }
    
    void Start()
    {
        _intensity = light.GetComponent<Light>().intensity;
        light.GetComponent<Light>().intensity = 0;
    }

    public override void OnSystemUpdateBuilding()
    {
        if(HasResource(ResourceIdentifiers.ENERGY))
        {
            foreach(var building in neighbourBuildings)
            {
                building.ReceiveResource(ResourceIdentifiers.LIGHT, 1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isOn = (EnvironmentManager.GetInstance().timeOfDayFraction < 0.25f || EnvironmentManager.GetInstance().timeOfDayFraction > 0.75f) && HasResource(ResourceIdentifiers.ENERGY);
        if(isOn != _on)
        {
            // light.SetActive(isOn);
            StartCoroutine(SetTurnOn(isOn));
            _on = isOn;
        }
    }

    IEnumerator SetTurnOn(bool isOn) 
    {
        Debug.Log("turning on");
        float target = isOn ? _intensity : 0;
        Light lightComponent = light.GetComponent<Light>();
        bool hasReachedTarget = isOn ? (lightComponent.intensity >= _intensity) : (lightComponent.intensity <= 0);
        Debug.Log(hasReachedTarget);
        while(!hasReachedTarget)
        {
            lightComponent.intensity += Time.deltaTime*(isOn ? 1 : -1)*_intensity/_turnonDuration;
            hasReachedTarget = isOn ? (lightComponent.intensity >= _intensity) : (lightComponent.intensity <= 0);
            yield return null;
        }

        yield return null;
    }
}
