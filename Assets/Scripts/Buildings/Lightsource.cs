using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsource : Building
{
    // public GameObject[] lights;
    private float _turnonDuration = 1.6f;
    private float _curIntensity = 0;
    private float _maxIntensity = 4;
    private bool _on = false;
    
    protected override void InitializeResourceNeeds()
    {
        SetNeedsResource(ResourceIdentifiers.ENERGY);
    }
    
    void Start()
    {
        SetLightIntensities(0);
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

    public override void Reselect()
    {
        this.transform.Rotate(new Vector3(0, 120, 0));
    }

    // Update is called once per frame
    protected override void Update()
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
        float target = isOn ? _curIntensity : 0;
        
        bool hasReachedTarget = isOn ? (_curIntensity >= _maxIntensity) : (_curIntensity <= 0);
        while(!hasReachedTarget)
        {
            _curIntensity += Time.deltaTime*(isOn ? 1 : -1)*_maxIntensity/_turnonDuration;
            hasReachedTarget = isOn ? (_curIntensity >= _maxIntensity) : (_curIntensity <= 0);
            SetLightIntensities(_curIntensity);
            yield return null;
        }            

        yield return null;
    }

    private void SetLightIntensities(float intensity) 
    {
        Light[] lightComponents = GetComponentsInChildren<Light>();

        foreach(Light lightComponent in lightComponents) 
        {
            lightComponent.intensity = intensity;
        }
    }
}
