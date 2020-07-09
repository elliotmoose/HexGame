using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoDetail : MonoBehaviour
{
    public GameObject barMetricPrefab;

    public Transform displaySlot;
    private GameObject _displayObject;
    public Text metricTitleText;
    public Text metricDescriptionText;
    public Transform metricsContainer;

    public Button actionButton;
    public GameObject destroyButton;
    public Text actionButtonText;

    //upgrades
    public Transform ugpradeDisplaySlot;
    private GameObject _upgradeDisplayObject;
    private Building selectedBuilding;
    private BuildingMetaData upgradeData {
        get {
            if(!selectedBuilding)
            {
                return null;
            }
            return selectedBuilding.metaData.upgrade;
        }
    }
    public GameObject upgradeDetailContainer;
    public Text upgradeTitleText;
    public Text upgradeCostText;
    public Text upgradeMetricsText;
    public Text upgradeDescriptionText;

    private List<BarMetricUI> _displayedMetricUIs = new List<BarMetricUI>();
    
    // Start is called before the first frame update
    void Start()
    {
        Controls.GetInstance().OnSelectPlatform += (HexPlatform platform) => {
            Debug.Log("ok!");
            Building building = BuildingsManager.GetInstance().BuildingAtCoordinate(platform.coordinate);
            LoadData(building);
        };
        
        SetOpen(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMetricValues();

        if(_displayObject)
        {
            _displayObject.transform.Rotate(0, UIManager.ITEM_DISPLAY_ROTATION_SPEED * Time.deltaTime, 0);
        }
        
        if(_upgradeDisplayObject)
        {
            _upgradeDisplayObject.transform.Rotate(0, UIManager.ITEM_DISPLAY_ROTATION_SPEED * Time.deltaTime, 0);
        }
    }

    public void LoadData(Building building) 
    {
        selectedBuilding = building;
        if(building == null)
        {
            Debug.Log("Loaded empty building");
            SetOpen(false);
            return;
        }
        
        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(selectedBuilding.Action);

        SetOpen(true);
        UpdateDisplay();
    }

    private void UpdateDisplay() 
    {
        if(!selectedBuilding)
        {
            Debug.Log("No building selected!");
            return;
        }

        //metrics display
        metricTitleText.text = selectedBuilding.metaData.title;
        metricDescriptionText.text = selectedBuilding.metaData.description;

        //action button        
        actionButtonText.text = selectedBuilding.GetActionText();
        actionButton.gameObject.SetActive(actionButtonText.text != "");

        if(_displayObject)
        {
            GameObject.Destroy(_displayObject);
        }

        _displayObject = GameObject.Instantiate(selectedBuilding.metaData.displayPrefab, displaySlot, false);
        _displayObject.transform.localPosition = Vector3.zero;
        _displayObject.layer = LayerMask.NameToLayer("UI");
        foreach (Transform child in _displayObject.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("UI");
        }

        destroyButton.SetActive(selectedBuilding.CanDestroy());

        //upgrade display
        if(upgradeData)
        {
            upgradeDetailContainer.SetActive(true);
            upgradeTitleText.text = $"UPGRADE: {upgradeData.title}";    
            upgradeCostText.text = $"{upgradeData.price}";

            if (_upgradeDisplayObject)
            {
                GameObject.Destroy(_upgradeDisplayObject);
            }

            _upgradeDisplayObject = GameObject.Instantiate(upgradeData.displayPrefab, ugpradeDisplaySlot, false);
            _upgradeDisplayObject.transform.localPosition = Vector3.zero;
            _upgradeDisplayObject.layer = LayerMask.NameToLayer("UI");
            foreach (Transform child in _upgradeDisplayObject.transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("UI");
            }
        }
        else 
        {
            upgradeDetailContainer.SetActive(false);
            upgradeTitleText.text = "";    
            upgradeCostText.text = "";
        }        
    }

    private void UpdateMetricValues() 
    {   
        if(!selectedBuilding) 
        {
            return;
        }        

        List<Metric> metrics = selectedBuilding.GetMetrics();
        
        int diff = metricsContainer.childCount - metrics.Count;

        //more children, destroy
        if(diff > 0) 
        {   
            for(int i=0; i< diff; i++)
            {
                GameObject.Destroy(metricsContainer.GetChild(i).gameObject);
            }
        }
        else if (diff < 0) 
        {
            
            for(int i=0; i< Mathf.Abs(diff); i++)
            {
                GameObject.Instantiate(barMetricPrefab, metricsContainer.transform.position, Quaternion.identity, metricsContainer);            
            }
        }
        
        for(int i=0; i< metrics.Count; i++)
        {
            Metric metric = metrics[i];
            metricsContainer.GetChild(i).GetComponent<BarMetricUI>().SetMetric(metric);
        }        
    }

    public void Upgrade()
    {
        if(selectedBuilding && upgradeData)
        {
            Shop.GetInstance().Purchase(upgradeData, selectedBuilding.coordinate);
        }
        else 
        {
            Debug.Log("No selected huilding or no upgrade Data");
        }
    }
  
    public void DestroyBuilding()
    {
        if(selectedBuilding)
        {
            BuildingsManager.GetInstance().DestoryBuildingAtCoordinate(selectedBuilding.coordinate);
        }
    }

    public void SetOpen(bool isOpen)
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(isOpen);
        }
    }


    #region singleton
    private static InfoDetail _singleton;

    InfoDetail() {
        _singleton = this;
    }

    public static InfoDetail GetInstance() 
    {
        return _singleton;
    }

    #endregion
}
