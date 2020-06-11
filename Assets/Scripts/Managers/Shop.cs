using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shop : MonoBehaviour
{
    public GameObject shopItemUIPrefab;
    public Transform shopItemsContainer;

    public bool isOpen {
        get {
            return this.gameObject.activeSelf;
        }
    }

    public HexPlatform selectedPlatform;
    
    public void Open(HexPlatform platform) 
    {
        if(isOpen && platform == selectedPlatform)
        {
            platform.Reselect();
            // Close();
            return;
        }

        if(!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
        }
       
       //set state to be selecting this tile 
       if(selectedPlatform) 
       {
           selectedPlatform.SetSelected(false);
       }
       selectedPlatform = platform;
       platform.SetSelected(true);

        UpdateShopItems();
    }

    public void Close() 
    {
        if(selectedPlatform) 
        {
            selectedPlatform.SetSelected(false);
        }

        this.gameObject.SetActive(false);
        selectedPlatform = null;
    }


    public List<ObjectMetaData> AvailableShopItemsForPlatform(HexPlatform platform) 
    {
        List<ObjectMetaData> shopItemsIds = new List<ObjectMetaData>();
        if(platform.building != null)
        {
            return shopItemsIds;
        }

        GameObject feature = MapManager.GetInstance().FeatureAtCoordinate(platform.coordinate);
        if(feature)
        {
            shopItemsIds.Add(MetaDataManager.MetaDataForId(Identifiers.MINING_PLATFORM));
            return shopItemsIds;
        }

        return platform.metaData.availableShopItems;
    }

    public void UpdateShopItems()
    {
        if(!selectedPlatform) 
        {
            return;
        }

        List<ObjectMetaData> metaDatas = AvailableShopItemsForPlatform(selectedPlatform);
                         
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

    public void Purchase(ObjectMetaData item) 
    {
        if(selectedPlatform == null) 
        {            
            Debug.LogError("Tried to purchase but no selected platform");
            return; 
        }

        GameObject platform = PlatformManager.GetInstance().Build(item, selectedPlatform.coordinate);
        if(platform != null) {
            Player.GetInstance().TransactMinerals(-item.price);
            
            HexPlatform hex = platform.GetComponent<HexPlatform>();

            if (hex != null)
            {
                selectedPlatform = hex;
            }
        }

        UpdateShopItems();
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
