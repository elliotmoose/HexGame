using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsource : Building
{
    // public GameObject[] lights;
    private float _turnonDuration = 1.6f;
    private float _curIntensity = 0;
    private float _maxIntensity = 4;
    private float idealEnergyInput = 30;
    private bool _on = false;
    private int axis = 0;
    
    protected override void InitializeResourceNeeds()
    {
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
            foreach(var building in neighbourBuildings)
            {
                building.ReceiveResource(ResourceIdentifiers.LIGHT, 1);
            }
        }

        //update brightness
        SetTurnOn(_on);
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
        bool isOn = (EnvironmentManager.GetInstance().timeOfDayFraction < 0.25f || EnvironmentManager.GetInstance().timeOfDayFraction > 0.75f) && HasResource(ResourceIdentifiers.ENERGY);
        if(isOn != _on)
        {
            // light.SetActive(isOn);
            SetTurnOn(isOn);
            _on = isOn;
        }

        foreach(Building neighbour in GetNeighbourBuildingsWithAxis(axis))
        {
            if(neighbour)
            {
                neighbour.ReceiveResource(ResourceIdentifiers.LIGHT, _curIntensity);
            }
        }

        base.Update();
    }
    

    Coroutine lightCoroutine = null;
    private void SetTurnOn(bool isOn)
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
        return $"Energy Input: {GetResource(ResourceIdentifiers.ENERGY)}\nLight Output: {_curIntensity}\nEnergy Fulfillment: {GetResourceFulfillFactor(ResourceIdentifiers.ENERGY)}";
    }
}
