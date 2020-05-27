using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject shopItemPrefab;
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
            Close();
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

       List<ShopItem> shopItems = platform.GetShopItems();
                     
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

    public void Close() 
    {
        if(selectedPlatform) 
        {
            selectedPlatform.SetSelected(false);
        }

        this.gameObject.SetActive(false);
        selectedPlatform = null;
    }

    public void Purchase(ShopItem item) 
    {
        if(selectedPlatform) 
        {
            Debug.Log(item.title);
        }
        else 
        {
            Debug.LogError("Tried to purchase but no selected platform");
        }
    }

    public void BuildPlatform(PlatformType platformType, Vector2Int coord)
    {
        if (PlatformManager.GetInstance().BuildPlatform(platformType, coord))
        {
            Player.GetInstance().TransactMinerals(-100);
        }
    }

    public void BuildAttachment(AttachmentType attachmentType, Vector2Int coord) 
    {
        HexPlatform tile = PlatformManager.GetInstance().PlatformAtCoordinate(coord);
        tile.BuildAttachment(AttachmentType.HARVESTER);
    }

    public void BuildBuilding(BuildingType buildingType, Vector2Int coord) 
    {
        HexPlatform tile = PlatformManager.GetInstance().PlatformAtCoordinate(coord);
        tile.BuildBuilding(BuildingType.TREE);
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
