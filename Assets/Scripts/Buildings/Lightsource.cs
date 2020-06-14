using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsource : Building
{
    // public GameObject[] lights;
    private float _turnonDuration = 1.6f;
    private float _curIntensity = 0;
    private float _maxIntensity = 4;
    private float idealEnergyInput;
    private float idealLightOutput;
    private bool _on = false;
    private int axis = 0;
    
    protected override void InitializeResourceNeeds()
    {
        metaData.MapParameterForKey("ENERGY_INPUT_IDEAL", out idealEnergyInput);
        metaData.MapParameterForKey("LIGHT_OUTPUT_IDEAL", out idealLightOutput);
        SetNeedsResource(ResourceIdentifiers.ENERGY, idealEnergyInput);
        AddResourceIndicator(ResourceIdentifiers.ENERGY, "Light source needs energy!");
    }
    
    void Start()
    {
        SetLightIntensities(0);
    }
    
    public override void RecalculateResources()
    {
        if(HasResource(ResourceIdentifiers.ENERGY))
        {            
            foreach(Building neighbour in GetNeighbourBuildingsWithAxis(axis))
            {
                if(neighbour)
                {
                    neighbour.ReceiveResource(ResourceIdentifiers.LIGHT, ScaledOutputByResource(ResourceIdentifiers.ENERGY, idealLightOutput));
                }
            }
        }

        //update brightness
        TriggerBrightnessUpdate(_on);
        base.RecalculateResources();
    }

    public override void Reselect()
    {
        axis = (axis + 1) % 3;
        this.transform.rotation = Quaternion.Euler(0, axis * 120, 0);
    }

    // Update is called once per frame
    protected override void Update()
    {
        bool isOn = !EnvironmentManager.GetInstance().isDay && HasResource(ResourceIdentifiers.ENERGY);
        if(isOn != _on)
        {
            TriggerBrightnessUpdate(isOn);
            _on = isOn;

            if(isOn)
            {
                GetResource(ResourceIdentifiers.LIGHT).active = true;
            }
            else 
            {
                GetResource(ResourceIdentifiers.LIGHT).active = false;
            }
        }
    }
    

    Coroutine lightCoroutine = null;
    private void TriggerBrightnessUpdate(bool isOn)
    {
        if(lightCoroutine != null)
        {
            StopCoroutine(lightCoroutine);
        }

        lightCoroutine = StartCoroutine(_SetTurnOn(isOn));
    }

    private IEnumerator _SetTurnOn(bool isOn) 
    {
        float target = isOn ? ScaledOutputByResource(ResourceIdentifiers.ENERGY, _maxIntensity) : 0;        
        float delta = target-_curIntensity;
        float time = 0;
        while(time < _turnonDuration)
        {
            _curIntensity += Time.deltaTime*delta/_turnonDuration;
            SetLightIntensities(_curIntensity);
            time += Time.deltaTime;
            yield return null;
        }            

        _curIntensity = target;
        SetLightIntensities(_curIntensity);

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

    public override string GetDescription()
    {                
        return $"Energy Input: {GetResource(ResourceIdentifiers.ENERGY)}\nLight Output: {_curIntensity}\nEnergy Fulfillment: {GetResource(ResourceIdentifiers.ENERGY).fulfillFactor}";
    }
}
