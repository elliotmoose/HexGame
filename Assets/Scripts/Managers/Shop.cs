using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject shopItemPrefab;
    public Transform shopItemsContainer;
    public Dictionary<Identifiers, ShopItem> shopItems = new Dictionary<Identifiers, ShopItem>();
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

    public void UpdateShopItems()
    {
        if(!selectedPlatform) 
        {
            return;
        }

        List<ShopItem> shopItems = selectedPlatform.GetShopItems();
                     
       //update ui based on shopItems
       for (int i = 0; i < shopItemsContainer.childCount; i++)
       {
           GameObject child = shopItemsContainer.GetChild(i).gameObject;
           GameObject.Destroy(child);
       }

       foreach (var item in shopItems)
       {
           GameObject shopItemUIObject = GameObject.Instantiate(shopItemPrefab, shopItemsContainer, false);           
           shopItemUIObject.GetComponent<ShopItemUI>().LoadData(item);
       }
    }

    public void Purchase(ShopItem item) 
    {
        if(selectedPlatform == null) 
        {            
            Debug.LogError("Tried to purchase but no selected platform");
            return; 
        }

        GameObject platform = PlatformManager.GetInstance().Build(item.id, selectedPlatform.coordinate);
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
