using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float minerals = 2000;
    public float oil = 500;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TransactMinerals(float amount) 
    {
        minerals += amount;

        UIManager.GetInstance().UpdateUI();
    }

    private static Player _singleton;

    Player() {
        _singleton = this;
    }

    public static Player GetInstance() 
    {
        return _singleton;
    }
}
