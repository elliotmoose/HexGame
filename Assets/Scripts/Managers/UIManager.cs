using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text mineralsText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI() 
    {
        mineralsText.text = $"{Player.GetInstance().minerals}";
    }

    private static UIManager _singleton;

    UIManager() {
        _singleton = this;
    }

    public static UIManager GetInstance() 
    {
        return _singleton;
    }
}
