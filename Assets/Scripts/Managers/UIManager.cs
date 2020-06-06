using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text mineralsText;
    public Text temperatureText;
    public Text co2Text;
    public GameObject detailsPanel;
    private HexPlatform _focusedPlatform = null;
    public RectTransform shopScrollViewContentContainer;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {        
        UpdateDetailsPanel();
    }

    public void UpdateUI() 
    {
        mineralsText.text = $"{Player.GetInstance().minerals}";
        
        temperatureText.text = Numbers.TwoDecimalPlace(EnvironmentManager.GetInstance().currentTemperature)+"°C";
        co2Text.text = Numbers.TwoDecimalPlace(EnvironmentManager.GetInstance().currentCO2)+"%";
    }

    private void UpdateDetailsPanel()
    {
        if(Shop.GetInstance().selectedPlatform != null) 
        {
            if(_focusedPlatform != Shop.GetInstance().selectedPlatform)
            {
                DisplayDetailsForPlatform(Shop.GetInstance().selectedPlatform);
            }
        }
        else 
        {
            _focusedPlatform = null;
            detailsPanel.SetActive(false);
        }
    }   

    private void DisplayDetailsForPlatform(HexPlatform platform) 
    {
        detailsPanel.SetActive(true);
        detailsPanel.GetComponentInChildren<Text>().text = $"{platform.GetDescription()}";
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
