using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static float ITEM_DISPLAY_ROTATION_SPEED = 75;
    public GameObject popupTextPrefab;

    public Text timeText;
    public Text mineralsText;
    public Text oilText;
    public Text temperatureText;
    public Text co2Text;
    
    public GameObject detailsContainer;
    public BuildingMetaData currentShopItemData;

    private HexTile _focusedPlatform = null;
    public RectTransform shopScrollViewContentContainer;

    public Camera UICamera;

    // Start is called before the first frame update
    void Start()
    {
        UpdateMetrics();
    }

    // Update is called once per frame
    void Update()
    {        
        UpdateMetrics();
        timeText.text = $"{Numbers.OneDP(EnvironmentManager.GetInstance().timeOfDay/EnvironmentManager.DAY_IN_SECONDS)}";
    }

    public void HoverDetailsEnter(BuildingMetaData shopItem, Vector3 targetPos) 
    {
        detailsContainer.SetActive(true);
        currentShopItemData = shopItem;
        detailsContainer.GetComponent<ShopItemDetail>().LoadData(shopItem);

        // Vector2 movePos;    
        
        // RectTransformUtility.ScreenPointToLocalPointInRectangle(GetCanvas().transform as RectTransform, Input.mousePosition, UICamera, out movePos);        
        // detailsContainer.transform.position = GetCanvas().transform.TransformPoint(movePos);
        detailsContainer.transform.position = targetPos + new Vector3(-0.3f,-0.3f,-2);
    }
    
    public void HoverDetailsExit(BuildingMetaData shopItem) 
    {
        if(currentShopItemData == shopItem)
        {
            detailsContainer.SetActive(false);
            currentShopItemData = null;
        }
    }

    public void UpdateMetrics() 
    {
        // mineralsText.text = $"{Numbers.TwoDP(Player.GetInstance().minerals)}";
        // oilText.text = $"{Numbers.TwoDP(Player.GetInstance().oil)}";
        mineralsText.text = $"{(int)Player.GetInstance().GetResource(ResourceIdentifiers.MINERALS)}";
        oilText.text = $"{(int)Player.GetInstance().GetResource(ResourceIdentifiers.OIL)}";
        
        temperatureText.text = Numbers.OneDP(EnvironmentManager.GetInstance().currentTemperature)+"°C";
        co2Text.text = Numbers.OneDP(EnvironmentManager.GetInstance().currentCO2)+"%";
    }

    private void DisplayDetailsForPlatform(HexTile platform) 
    {
        // detailsPanel.SetActive(true);
        // detailsPanel.GetComponentInChildren<Text>().text = $"{platform.GetDescription()}";
    }

    public static void Message(string message, int style=0)
    {
        Debug.Log(message);
    }

    public static void PopupText(string text, GameObject target) 
    {
        GameObject popupTextGo = GameObject.Instantiate(_singleton.popupTextPrefab, Vector3.zero, Quaternion.identity, GetCanvas().transform);
        popupTextGo.GetComponent<PopupText>().Initialize(text, target);
        
    }

    #region singleton
    private static UIManager _singleton;

    UIManager() {
        _singleton = this;
    }

    public static UIManager GetInstance() 
    {
        return _singleton;
    }

    #endregion
    
    public static GameObject GetCanvas() 
    {
        return _singleton.gameObject;
    }

    public static Vector3 WorldToUISpace(Vector3 worldPos)
    {
        return UIHelper.WorldToUISpace(worldPos, _singleton.GetComponent<Canvas>(), _singleton.UICamera);
    }
}
