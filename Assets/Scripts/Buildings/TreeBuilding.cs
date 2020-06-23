using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBuilding : Building
{
    public Color healthBarGreen;
    public Color healthBarOrange;
    public Color healthBarRed;
    public GameObject healthBarObject;
    public GameObject treeObject;
    public GameObject smokeObject;
    public GameObject fireObject;
    public GameObject leavesObject;
    public GameObject trunkObject;
    public Gradient leaveColors;

    private float _maxAngle = 40;

    private float _maxO2Production = 0.005f;
    private bool hasLight {
        get {
            return HasResource(ResourceIdentifiers.LIGHT);
        }
    }
    
    private bool hasWater {
        get {
            return HasResource(ResourceIdentifiers.WATER);
        }
    }

    private float _maxAge = 15;
    private float _curAge = 0;

    private float _health = 100;
    private float _maxHealth = 100;
    private float _healthRegenRate = 2;
    public bool alive = true;
    
    private float _coolFactorTemp = 0;
    private float _baseTemp = 25;
    private float _burnTemp = 0;
    private int _burnStage = 0; //0 = not burning, 1 = smoking, 2 = burn

    private float _coolingRate;
    private float _smokingHeatRate;
    private float _burningHeatRate;
    private float _noLightDamage;
    private float _noWaterDamage;
    private float _burnDamage;
    private float _internalTemperature {
        get {                        
            return _baseTemp + _burnTemp + _coolFactorTemp;
        }
    }  

    private bool isBurning {
        get {
            return _burnStage != 0;
        }
    }

    private float _smokePoint;    
    private float _burnPoint;    

    private float idealWaterInput;
    private float idealLightInput;

    void Awake()
    {
        treeObject.transform.localScale = Vector3.zero;
    }

    public void GrowUp()
    {
        _curAge = _maxAge;    
        treeObject.transform.localScale = new Vector3(1,1,1);
        treeObject.transform.rotation = Quaternion.Euler(0, _maxAngle, 0);
    }

    protected override void InitializeResourceNeeds()
    {
        MetaDataParameter waterInputParameter = metaData.GetParameterForKey("WATER_INPUT_IDEAL");
        idealWaterInput = waterInputParameter.value;
        MetaDataParameter lightInputParameter = metaData.GetParameterForKey("LIGHT_INPUT_IDEAL");
        idealLightInput = lightInputParameter.value;
        MetaDataParameter coolInputParameter = metaData.GetParameterForKey("COOL_FACTOR");
        metaData.MapParameterForKey("COOLING_RATE", out _coolingRate);
        metaData.MapParameterForKey("SMOKING_HEAT_RATE", out _smokingHeatRate);
        metaData.MapParameterForKey("BURNING_HEAT_RATE", out _burningHeatRate);
        metaData.MapParameterForKey("SMOKE_POINT", out _smokePoint);
        metaData.MapParameterForKey("BURN_POINT", out _burnPoint);
        metaData.MapParameterForKey("NO_WATER_DAMAGE", out _noWaterDamage);
        metaData.MapParameterForKey("NO_LIGHT_DAMAGE", out _noLightDamage);
        metaData.MapParameterForKey("BURN_DAMAGE", out _burnDamage);

        SetNeedsResource(ResourceIdentifiers.WATER, waterInputParameter);
        SetNeedsResource(ResourceIdentifiers.LIGHT, lightInputParameter);
        SetNeedsResource(ResourceIdentifiers.COOL, coolInputParameter);

        AddResourceIndicator(ResourceIdentifiers.WATER, "This tree needs water!!");
        AddResourceIndicator(ResourceIdentifiers.LIGHT, "This tree needs light!!");
    }

    protected override void Update() 
    {        
        
        if(!hasLight)
        {
            Decay(_noLightDamage);
        }
        
        if(!hasWater)
        {
            Decay(_noWaterDamage);
        }

        if(hasLight && hasWater && alive) {
            Grow();
        }
            
        UpdateLeaveAndHealthbarColors();
        UpdateInternalTemperature();
        CheckBurn();     
    }

    private void Decay(float amount) 
    {
        if(_health > 0)
        {
            _health -= amount * Time.deltaTime;            
        }
        else 
        {
            Die();
        }
    }

    private void Grow() 
    {        
        if(_curAge < _maxAge)
        {
            _curAge += Time.deltaTime;
            float ageFactor = _curAge/_maxAge;
            treeObject.transform.localScale = new Vector3(ageFactor, ageFactor, ageFactor);

            treeObject.transform.rotation = Quaternion.Euler(0, ageFactor * _maxAngle, 0);
        }

        if(!isBurning)
        {   
            if( _health <= _maxHealth)
            {
                _health += _healthRegenRate * Time.deltaTime;
            }
        }
    }

    private void UpdateInternalTemperature()
    {        
        if(HasResource(ResourceIdentifiers.COOL))
        {
            _coolFactorTemp = -GetResource(ResourceIdentifiers.COOL).value;
        }

        //environment base temp
        float environmentTemp = EnvironmentManager.GetInstance().currentTemperature;
        float targetTemp = environmentTemp + _coolFactorTemp; //so that equilibrium is at cooled temp

        _baseTemp += (targetTemp-_internalTemperature)/10 * Time.deltaTime;

        switch (_burnStage)
        {
            case 0:
                _burnTemp = Mathf.Max(_burnTemp - (Time.deltaTime * _coolingRate), 0);
                break;
            
            case 1:
                _burnTemp += (Time.deltaTime * _smokingHeatRate);
                break;
            
            case 2:
                _burnTemp += (Time.deltaTime * _burningHeatRate);
                break;

            default:
                break;
        }
    }

    private void CheckBurn() 
    {
        if(_internalTemperature < _smokePoint) 
        {
            SetBurnStage(0);
            return;
        }

        switch (_burnStage)
        {
            case 0:
                if(_internalTemperature >= _smokePoint) 
                {
                    SetBurnStage(1);
                }
                break;
            case 1:
                if(_internalTemperature >= _burnPoint) 
                {
                    SetBurnStage(2);
                }
                break;
            case 2:
                Decay(_burnDamage);
                break;
            default:
                break;
        }
    }



    public override void BuildingTick()
    {
        if(alive && _burnStage == 0 && hasLight)
        {
            float o2Produced = _maxO2Production * (_curAge/_maxAge);
            EnvironmentManager.GetInstance().ProduceO2(o2Produced);
            UIManager.PopupText($"{o2Produced}", this.gameObject);
        }
    }


    public override void ResetResources()
    {
        ExpendAllResource(ResourceIdentifiers.COOL);
        ExpendAllResource(ResourceIdentifiers.LIGHT);
        ExpendAllResource(ResourceIdentifiers.WATER);
    }


    private void SetBurnStage(int stage)
    {
        var smokeEmission = smokeObject.GetComponent<ParticleSystem>().emission;
        var fireEmission = fireObject.GetComponent<ParticleSystem>().emission;
        _burnStage = stage;
        if(stage == 0) 
        {
            smokeEmission.enabled = false;            
            fireEmission.enabled = false;            
        }
        else if(stage == 1)
        {
            smokeEmission.enabled = true;
            fireEmission.enabled = false;            
        }
        else if(stage == 2)
        {
            smokeEmission.enabled = false;
            fireEmission.enabled = true;            
        }
    }



    private void UpdateLeaveAndHealthbarColors() 
    {
        leavesObject.GetComponent<Renderer>().material.color = leaveColors.Evaluate(_health/_maxHealth);


        Color healthBarColor;

        if(_health/_maxHealth > 0.7f)
        {
            healthBarColor = healthBarGreen;
        }
        else if(_health/_maxHealth > 0.3f) 
        {
            healthBarColor = healthBarOrange;
        }
        else 
        {
            healthBarColor = healthBarRed;
        }

        healthBarObject.GetComponent<Renderer>().material.color = healthBarColor;
    }

    private void Die() 
    {
        alive = false;
        leavesObject.SetActive(false);
        trunkObject.GetComponent<Renderer>().material.color = new Color32(0,0,0,1);

    }

    public override string GetDescription() 
    {        
        return $"Health: {_health}\nInternal Temp: {_internalTemperature}\nBurning: {_burnStage}\nSmokePt: {_smokePoint}\nFlashPt: {_burnPoint}\nProducing O2: {alive && _burnStage == 0 && hasLight}\nCool Factor: {_coolFactorTemp}";
    }
}