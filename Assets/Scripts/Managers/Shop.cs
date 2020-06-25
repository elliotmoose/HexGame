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
    
    public Material validBuildMaterial;
    public Material invalidBuildMaterial;
    
    public bool isOpen = false;
    
    public List<ObjectMetaData> resourcesShopItems = new List<ObjectMetaData>();
    public List<ObjectMetaData> platformsShopItems = new List<ObjectMetaData>();
    public List<ObjectMetaData> energyShopItems = new List<ObjectMetaData>();
    public List<ObjectMetaData> organicShopItems = new List<ObjectMetaData>();

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
        
        for(int i=0; i<categoriesContainer.childCount; i++)
        {
            Button button = categoriesContainer.GetChild(i).GetComponent<Button>();
            bool isSelected = (i == index);
            ColorBlock colors = button.colors;
            colors.normalColor = isSelected ? buttonDarkSelected : buttonDark;
            button.colors = colors;            
        }

        List<ObjectMetaData>[] categories = new List<ObjectMetaData>[]{resourcesShopItems, platformsShopItems, energyShopItems, organicShopItems};
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


    public List<ObjectMetaData> AvailableShopItemsForPlatform(HexPlatform platform) 
    {
        List<ObjectMetaData> shopItemsIds = new List<ObjectMetaData>();
        if(platform.building != null)
        {
            return shopItemsIds;
        }

        return platform.metaData.GetContextualAvailableShopItems(platform.coordinate);
    }

    public void UpdateShopItems(List<ObjectMetaData> metaDatas)
    {
                         
       //clear shop item uis
       for (int i = 0; i < shopItemsContainer.childCount; i++)
       {
           GameObject child = shopItemsContainer.GetChild(i).gameObject;
           GameObject.Destroy(child);
       }

        //rebuild shop item uis
       foreach (ObjectMetaData shopItemMetaData in metaDatas)
       {
           GameObject shopItemUIObject = GameObject.Instantiate(shopItemUIPrefab, shopItemsContainer, false);           
           shopItemUIObject.GetComponent<ShopItemUI>().LoadData(shopItemMetaData);
       }
    }

    public void Purchase(ObjectMetaData item, Vector2Int coord) 
    {        
        if(PlatformManager.GetInstance().PlatformAtCoordinate(coord) == null) 
        {            
            Debug.LogError("Tried to purchase but no target platform");
            return; 
        }

        if(item.price > Player.GetInstance().minerals)
        {
            Debug.LogWarning("Insufficient Minerals");
            return;
        }

        GameObject platform = PlatformManager.GetInstance().Build(item, coord);
        if(platform != null) {
            Player.GetInstance().TransactResource(ResourceIdentifiers.MINERALS, -item.price);            
        }


        // UpdateShopItems();
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
