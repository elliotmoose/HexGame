using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AttachmentType {
    HARVESTER
}

public class HexPlatform : ResourceConsumer
{
    public Identifiers id = Identifiers.NULL;
    public Vector2Int coordinate;

    public Material defaultMaterial;
    public Material hoverMaterial;
    public Material selectedMaterial;

    private Building _building;
    public Building building {
        get {
            return _building;
        }

        set {
            _building = value;
        }
    }

    public bool isSelected {
        get {
            return Shop.GetInstance().selectedPlatform == this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!defaultMaterial)
        {
            defaultMaterial = GetComponentInChildren<Renderer>().material;
        }
    }

    public void Initialize(Identifiers id, Vector2Int coord) {
        this.id = id;
        coordinate = coord;
        InitializeResourceNeeds();
    }

    public void SetHovered(bool isHovered) {
        if(id == Identifiers.NULL) 
        {
            return;
        }
        

        
        Material material = isHovered ? hoverMaterial : (isSelected ? selectedMaterial : defaultMaterial);
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = material;
        }
    }

    public virtual void SetSelected(bool isHovered) {
        if(id == Identifiers.NULL) 
        {
            return;
        }
        
        Material material = isHovered ? hoverMaterial : defaultMaterial;
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = material;
        }
    }

    public void Tick() 
    {
        if(building != null)
        {
            building.BuildingTick();
        }

        PlatformTick();
    }

    public void OnSystemUpdate() 
    {
        if(building != null) 
        {
            building.OnSystemUpdateBuilding();
        }

        OnSystemUpdatePlatform();
    }

    protected virtual void PlatformTick() 
    {
        
    }

    protected virtual void OnSystemUpdatePlatform() 
    {
        
    }

    public virtual void Reselect() 
    {
        if(building)
        {
            building.Reselect();
        }
    }

    public List<ShopItem> GetShopItems() 
    {
        List<ShopItem> shopItems = new List<ShopItem>();
        if(_building != null)
        {
            return shopItems;
        }

        GameObject feature = MapManager.GetInstance().FeatureAtCoordinate(this.coordinate);
        if(feature)
        {
            shopItems.Add(ShopItem.MiningPlatform());
            return shopItems;
        }

        switch (this.id)
        {
            case Identifiers.PLACEHOLDER_PLATFORM:
                shopItems.Add(ShopItem.StonePlatform());
                shopItems.Add(ShopItem.SoilPlatform());
                shopItems.Add(ShopItem.DigSitePlatform());
                break;
            case Identifiers.SOIL_PLATFORM:
                shopItems.Add(ShopItem.Tree());
                break;
            case Identifiers.STONE_PLATFORM:
                shopItems.Add(ShopItem.Condenser());
                shopItems.Add(ShopItem.LightSource());
                shopItems.Add(ShopItem.Generator());
                shopItems.Add(ShopItem.Turbine());
                break;
            case Identifiers.DIG_SITE_PLATFORM:
                shopItems.Add(ShopItem.MineralMiner());
                shopItems.Add(ShopItem.OilPump());
                break;
            default:
                return shopItems;
        }


        return shopItems;
    }

    public virtual string GetDescription()
    {
        string description = $"{id}\n";
        if(building)
        {
            description += building.GetDescription();
        }
        return description;
    }
}
