using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public GameObject indicatorSet;

    //platforms
    public GameObject emptyPlatform;
    public GameObject placeholderPlatform;
    public GameObject miningPlatform;
    public GameObject stonePlatform;
    public GameObject soilPlatform;

    public GameObject miningDisplay;
    public GameObject stoneDisplay;
    public GameObject soilDisplay;
    
    //buildings
    public GameObject tree;
    public GameObject condenser;
    public GameObject lightsource;
    public GameObject generator;
    public GameObject turbine;

    public GameObject treeDisplay;
    public GameObject condenserDisplay;
    public GameObject lightsourceDisplay;
    public GameObject generatorDisplay;
    public GameObject turbineDisplay;
    
    
    public Sprite nowater;
    public Sprite noenergy;
    public Sprite nolight;
    
    private static PrefabManager _singleton;

    PrefabManager() {
        _singleton = this;
    }

    public static PrefabManager GetInstance() 
    {
        return _singleton;
    }

    public static GameObject PrefabForID(Identifiers id) 
    {        
        switch (id)
        {   
            //platforms
            case Identifiers.EMPTY_PLATFORM:
                return _singleton.emptyPlatform;
            case Identifiers.PLACEHOLDER_PLATFORM:
                return _singleton.placeholderPlatform;
            case Identifiers.SOIL_PLATFORM:
                return _singleton.soilPlatform;
            case Identifiers.STONE_PLATFORM:
                return _singleton.stonePlatform;
            case Identifiers.MINING_PLATFORM:
                return _singleton.miningPlatform;
            //buildings
            case Identifiers.TREE_BUILDING:
                return _singleton.tree;
            case Identifiers.CONDENSER_BUILDING:
                return _singleton.condenser;
            case Identifiers.GENERATOR_BUILDING:
                return _singleton.generator;
            case Identifiers.LIGHTSOURCE_BUILDING:
                return _singleton.lightsource;
            case Identifiers.TURBINE_BUILDING:
                return _singleton.turbine;
            
            default:
                Debug.LogWarning($"Can't find prefab for id: {id}");
                return null;
        }
    }

    public static Sprite SpriteForResourceId(ResourceIdentifiers id) 
    {
        switch (id)
        {
            case ResourceIdentifiers.LIGHT:
                return _singleton.nolight;
            case ResourceIdentifiers.WATER:
                return _singleton.nowater;
            case ResourceIdentifiers.ENERGY:
                return _singleton.noenergy;
            default:
                return null;
        }
    }
}
