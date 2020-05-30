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
        if(selectedPlatform) 
        {            
            //PLATFORMS CAN ONLY BE BUILT ON PLACEHOLDERS
            if(selectedPlatform.platformType == PlatformType.PLACEHOLDER)
            {
                switch (item.id)
                {
                    case ShopItemID.STONE_PLATFORM:
                        selectedPlatform = BuildPlatform(PlatformType.STONE, selectedPlatform.coordinate);
                        break;
                    case ShopItemID.SOIL_PLATFORM:
                        selectedPlatform = BuildPlatform(PlatformType.SOIL, selectedPlatform.coordinate);
                        break;
                    case ShopItemID.MINING_PLATFORM:
                        selectedPlatform = BuildPlatform(PlatformType.MINING, selectedPlatform.coordinate);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (item.id)
                {
                    case ShopItemID.TREE_BUILDING:
                        BuildBuilding(BuildingType.TREE, selectedPlatform.coordinate);
                        break;
                    case ShopItemID.LIGHTSOURCE_BUILDING:
                        BuildBuilding(BuildingType.LIGHTSOURCE, selectedPlatform.coordinate);
                        break;
                    case ShopItemID.CONDENSER_BUILDING:
                        BuildBuilding(BuildingType.CONDENSER, selectedPlatform.coordinate);
                        break;
                    case ShopItemID.GENERATOR_BUILDING:
                        BuildBuilding(BuildingType.GENERATOR, selectedPlatform.coordinate);
                        break;
                    default:
                        break;
                }
            }

            UpdateShopItems();
        }
        else 
        {
            Debug.LogError("Tried to purchase but no selected platform");
        }
    }

    private HexPlatform BuildPlatform(PlatformType platformType, Vector2Int coord)
    {
        HexPlatform platform = PlatformManager.GetInstance().BuildPlatform(platformType, coord);
        if (platform)
        {
            Player.GetInstance().TransactMinerals(-100);
        }
        
        return platform;
    }

    // private void BuildAttachment(AttachmentType attachmentType, Vector2Int coord) 
    // {
    //     HexPlatform tile = PlatformManager.GetInstance().PlatformAtCoordinate(coord);
    //     tile.BuildAttachment(AttachmentType.HARVESTER);
    // }

    public void BuildBuilding(BuildingType buildingType, Vector2Int coord) 
    {
        HexPlatform tile = PlatformManager.GetInstance().PlatformAtCoordinate(coord);
        tile.BuildBuilding(buildingType);
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
