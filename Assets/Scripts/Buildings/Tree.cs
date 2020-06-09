using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Building
{
    public GameObject treeObject;
    public GameObject smokeObject;
    public GameObject fireObject;
    public GameObject leavesObject;
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
    private float _healthRegenRate = 1;
    public bool alive = true;
    
    private float _coolFactorTemp = 0;
    private float _baseTemp  = 25;
    private float _burnTemp = 0;
    private float _burnTempCoolRate = 1;
    private float _smokeTempHeatRate = 3;
    private float _burnTempHeatRate = 10;
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

    private float _smokePoint = 48;    
    private float _flashPoint = 65;    
    private int _burnStage = 0; //0 = not burning, 1 = smoking, 2 = burn

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
        SetNeedsResource(ResourceIdentifiers.WATER);
        SetNeedsResource(ResourceIdentifiers.LIGHT);
        SetNeedsResource(ResourceIdentifiers.COOL);

        AddResourceIndicator(ResourceIdentifiers.WATER, "This tree needs water!!");
        AddResourceIndicator(ResourceIdentifiers.LIGHT, "This tree needs light!!");
    }

    protected override void Update() 
    {        
        base.Update();
        if(!hasLight || !hasWater) {
            Decay(1);
        }
        else 
        {
            Grow();
        }        

        UpdateInternalTemperature();
        CheckBurn();     
        UpdateResourceIndicators();   
    }

    private void Decay(float amount) 
    {
        if(_health > 0)
        {
            _health -= amount * Time.deltaTime;
            UpdateLeaveColors();
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
                _health += _healthRegenRate;
            }
        }
    }

    private void UpdateInternalTemperature()
    {
        if(HasResource(ResourceIdentifiers.COOL))
        {
            _coolFactorTemp = -ExpendAllResource(ResourceIdentifiers.COOL);
        }

        //environment base temp
        float environmentTemp = EnvironmentManager.GetInstance().currentTemperature;
        float targetTemp = environmentTemp + _coolFactorTemp; //so that equilibrium is at cooled temp

        _baseTemp += (targetTemp-_internalTemperature)/10 * Time.deltaTime;

        switch (_burnStage)
        {
            case 0:
                if(_burnTemp > 0)
                {
                    _burnTemp -= (Time.deltaTime * _burnTempCoolRate);
                }
                else 
                {
                    _burnTemp = 0;
                }
                break;
            
            case 1:
                _burnTemp += (Time.deltaTime * _smokeTempHeatRate);
                break;
            
            case 2:
                _burnTemp += (Time.deltaTime * _burnTempHeatRate);
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
                if(_internalTemperature >= _flashPoint) 
                {
                    SetBurnStage(2);
                }
                break;
            case 2:
                Decay(10);
                break;
            default:
                break;
        }
    }



    public override void BuildingTick()
    {
        if(alive && _burnStage == 0 && hasLight)
        {
            EnvironmentManager.GetInstance().ProduceO2(_maxO2Production * (_curAge/_maxAge));
            UIManager.GetInstance().UpdateUI();
        }

    }

    /// <summary>
    /// Checks if the platform this tree is on has the requirements it needs to grow
    /// </summary>
    public override void OnSystemUpdateBuilding()
    {
        //day time
        // ReceiveResource(ResourceIdentifiers.LIGHT, 1);
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

    private void UpdateResourceIndicators() 
    {
        SetResourceIndicator(ResourceIdentifiers.LIGHT, !HasResource(ResourceIdentifiers.LIGHT));
        SetResourceIndicator(ResourceIdentifiers.WATER, !HasResource(ResourceIdentifiers.WATER));        
    }

    private void UpdateLeaveColors() 
    {
        Color color = leaveColors.Evaluate(_health/_maxHealth);
        leavesObject.GetComponent<Renderer>().materials[0].color = color;
    }

    private void Die() 
    {
        alive = false;
    }

    public override string GetDescription() 
    {        
        return $"Health: {_health}\nInternal Temp: {_internalTemperature}\nBurning: {_burnStage}\nSmokePt: {_smokePoint}\nFlashPt: {_flashPoint}\nProducing O2: {alive && _burnStage == 0 && hasLight}\nCool Factor: {_coolFactorTemp}";
    }
}