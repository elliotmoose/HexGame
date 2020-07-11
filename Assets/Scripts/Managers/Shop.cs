using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    public GameObject shopItemUIPrefab;
    public Transform shopItemsContainer;
    public Transform categoriesContainer;
    public GameObject shopItemsDisplay;
    public Text shopCategoryText;
    
    public Material validBuildMaterial;
    public Material invalidBuildMaterial;
    
    public bool isOpen = false;
    
    public List<BuildingMetaData> resourcesShopItems = new List<BuildingMetaData>();
    public List<BuildingMetaData> platformsShopItems = new List<BuildingMetaData>();
    public List<BuildingMetaData> energyShopItems = new List<BuildingMetaData>();
    public List<BuildingMetaData> organicShopItems = new List<BuildingMetaData>();

    private int _selectedCategory = 0;

    public Color buttonDark;
    public Color buttonDarkSelected;

    void Start()
    {
        // DisplayCategory(0);
        SubscribeCategoryButtonsEvents();
    }

    public void SubscribeCategoryButtonsEvents()
    {
        for(int i=0; i<categoriesContainer.childCount; i++)
        {
            int index = i;
            categoriesContainer.GetChild(i).GetComponent<Button>().onClick.AddListener(()=>{
                DisplayCategory(index);
            });
        }
    }

    public void DisplayCategory(int index)
    {
        if(_selectedCategory == index && isOpen) 
        {
            SetOpen(false);
            return;
        }

        SetOpen(true);
        _selectedCategory = index;
        shopCategoryText.text = new string[]{"Resources", "Platforms", "Energy", "Organic"}[index];
        
        for(int i=0; i<categoriesContainer.childCount; i++)
        {
            Button button = categoriesContainer.GetChild(i).GetComponent<Button>();
            bool isSelected = (i == index);
            ColorBlock colors = button.colors;
            colors.normalColor = isSelected ? buttonDarkSelected : buttonDark;
            button.colors = colors;            
        }

        List<BuildingMetaData>[] categories = new List<BuildingMetaData>[]{resourcesShopItems, platformsShopItems, energyShopItems, organicShopItems};
        UpdateShopItems(categories[index]);
    }
    public void SetOpen(bool open) 
    {
        isOpen = open;
        if(!open)
        {
            _selectedCategory = -1;
        }

        //update ui panel
        shopItemsDisplay.SetActive(open);

        //category ui update
        if(!open)
        {
            for(int i=0; i<categoriesContainer.childCount; i++)
            {
                Button button = categoriesContainer.GetChild(i).GetComponent<Button>();
                ColorBlock colors = button.colors;
                colors.normalColor = buttonDark;
                button.colors = colors;            
            }
        }
    }

    public void UpdateShopItems(List<BuildingMetaData> metaDatas)
    {
                         
       //clear shop item uis
       for (int i = 0; i < shopItemsContainer.childCount; i++)
       {
           GameObject child = shopItemsContainer.GetChild(i).gameObject;
           GameObject.Destroy(child);
       }

        //rebuild shop item uis
       foreach (BuildingMetaData shopItemMetaData in metaDatas)
       {
           GameObject shopItemUIObject = GameObject.Instantiate(shopItemUIPrefab, shopItemsContainer, false);           
           shopItemUIObject.GetComponent<ShopItemUI>().LoadData(shopItemMetaData);
       }
    }

    public void Purchase(BuildingMetaData item, Vector2Int coord) 
    {        
        if(HexMapManager.GetInstance().TileAtCoordinate(coord) == null) 
        {            
            Debug.LogError("Tried to purchase but no target platform");
            return; 
        }

        if(!Player.GetInstance().CanAfford(item.costs))
        {
            return;
        }

        GameObject platform = BuildingsManager.GetInstance().Build(item, coord);
        if(platform != null) {
            Player.GetInstance().TransactCosts(item.costs);            
        }
    }

    private static Shop _singleton;

    public Shop() {
        _singleton = this;
    }

    public static Shop GetInstance() 
    {
        return _singleton;
    }
}
