﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoDetail : MonoBehaviour
{
    public GameObject barMetricPrefab;

    public Text metricTitleText;
    public Text metricDescriptionText;
    public Transform metricsContainer;

    public Button actionButton;
    public Text actionButtonText;

    //upgrades
    private Building selectedBuilding;
    private ObjectMetaData upgradeData;
    public GameObject upgradeDetailContainer;
    public Text upgradeTitleText;
    public Text upgradeCostText;
    public Text upgradeMetricsText;
    public Text upgradeDescriptionText;

    private List<BarMetricUI> _displayedMetricUIs = new List<BarMetricUI>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMetricValues();
    }

    public void LoadData(Building building) 
    {
        selectedBuilding = building;
        
        actionButton.onClick.AddListener(()=>{
            selectedBuilding.Action(); 
        });

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

        //upgrade display
        if(upgradeData)
        {
            upgradeDetailContainer.SetActive(true);
            upgradeTitleText.text = $"UPGRADE: {upgradeData.title}";    
            upgradeCostText.text = $"{upgradeData.price}";
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
