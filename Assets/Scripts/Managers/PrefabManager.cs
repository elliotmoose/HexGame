﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    //platforms
    public GameObject grassPlatform;
    public GameObject soilPlatform;

    public GameObject mineralAttachment;
    public GameObject tree;
    
    private static PrefabManager _singleton;

    PrefabManager() {
        _singleton = this;
    }

    public static PrefabManager GetInstance() 
    {
        return _singleton;
    }
}
