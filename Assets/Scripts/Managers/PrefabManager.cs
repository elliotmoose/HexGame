using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public GameObject indicatorSet;
    
    public Sprite nowater;
    public Sprite nooil;
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
