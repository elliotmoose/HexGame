using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
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

    public GameObject treeDisplay;
    public GameObject condenserDisplay;
    public GameObject lightsourceDisplay;
    public GameObject generatorDisplay;
    
    private static PrefabManager _singleton;

    PrefabManager() {
        _singleton = this;
    }

    public static PrefabManager GetInstance() 
    {
        return _singleton;
    }
}
