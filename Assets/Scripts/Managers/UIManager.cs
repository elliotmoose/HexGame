using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject popupTextPrefab;

    public Text timeText;
    public Text mineralsText;
    public Text oilText;
    public Text temperatureText;
    public Text co2Text;
    public GameObject detailsPanel;
    private HexPlatform _focusedPlatform = null;
    public RectTransform shopScrollViewContentContainer;

    public Camera UICamera;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {        
        UpdateUI();
        UpdateDetailsPanel();
        timeText.text = $"{Numbers.TwoDP(EnvironmentManager.GetInstance().timeOfDay/EnvironmentManager.DAY_IN_SECONDS)}";
    }

    public void UpdateUI() 
    {
        mineralsText.text = $"{Numbers.TwoDP(Player.GetInstance().minerals)}";
        oilText.text = $"{Numbers.TwoDP(Player.GetInstance().oil)}";
        
        temperatureText.text = Numbers.TwoDP(EnvironmentManager.GetInstance().currentTemperature)+"°C";
        co2Text.text = Numbers.TwoDP(EnvironmentManager.GetInstance().currentCO2)+"%";
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

    public static void PopupText(string text, GameObject target) 
    {
        GameObject popupTextGo = GameObject.Instantiate(_singleton.popupTextPrefab, Vector3.zero, Quaternion.identity, GetCanvas().transform);
        popupTextGo.GetComponent<PopupText>().Initialize(text, target);
        
    }

    private static UIManager _singleton;

    UIManager() {
        _singleton = this;
    }

    public static UIManager GetInstance() 
    {
        return _singleton;
    }

    public static GameObject GetCanvas() 
    {
        return _singleton.gameObject;
    }

    public static Vector3 WorldToUISpace(Vector3 worldPos)
    {
        Canvas parentCanvas = _singleton.GetComponent<Canvas>();
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, _singleton.UICamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }
}
