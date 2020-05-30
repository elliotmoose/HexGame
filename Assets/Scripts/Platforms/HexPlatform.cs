using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BuildingType {
    TREE,
    CONDENSER,
    LIGHTSOURCE,
    GENERATOR
}
public enum PlatformType {
    ROOT,
    PLACEHOLDER,
    MINING,
    STONE,
    SOIL,
    NONE
}

public enum AttachmentType {
    HARVESTER
}

public class HexPlatform : MonoBehaviour
{
    public PlatformType platformType = PlatformType.NONE;
    public Vector2Int coordinate;

    public Material defaultMaterial;
    public Material hoverMaterial;
    public Material selectedMaterial;

    public GameObject building;

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
            defaultMaterial = GetComponent<Renderer>().material;
        }
    }

    public void Initialize(PlatformType tileType, Vector2Int coord) {
        this.platformType = tileType;
        coordinate = coord;
    }

    public void SetHovered(bool isHovered) {
        if(platformType == PlatformType.NONE) 
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
        if(platformType == PlatformType.NONE) 
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

    public bool CanBuild() {
        return platformType == PlatformType.PLACEHOLDER;
    }

    // public void BuildAttachment(AttachmentType attachmentType) {
    //     if(attachmentType == AttachmentType.HARVESTER && platformType == PlatformType.MINERAL)
    //     {
    //         GameObject.Instantiate(PrefabManager.GetInstance().mineralAttachment, this.transform.position - new Vector3(0, 0.1f, 0 ), this.transform.rotation);
    //     }
    // }

    private bool CanBuildBuilding(BuildingType buildingType)
    {
        if(building != null)
        {
            Debug.LogWarning("Can't build building: Platform already has building");
            return false;
        }
        switch (buildingType)
        {
            case BuildingType.TREE:
                return platformType == PlatformType.SOIL;
            case BuildingType.CONDENSER:
                return platformType == PlatformType.STONE;
            case BuildingType.LIGHTSOURCE:
                return platformType == PlatformType.STONE;
            case BuildingType.GENERATOR:
                return platformType == PlatformType.STONE;
            default:
                return false;
        }
    }

    public void BuildBuilding(BuildingType buildingType) {

        if(!CanBuildBuilding(buildingType))
        {
            Debug.LogWarning($"Cannot build {buildingType} building on this {platformType} platform");
            return;
        }
        
        switch (buildingType)
        {
            case BuildingType.TREE:
                building = GameObject.Instantiate(PrefabManager.GetInstance().tree, this.transform.position - new Vector3(0, 0.1f, 0 ), this.transform.rotation);
                break;
            case BuildingType.CONDENSER:
                building = GameObject.Instantiate(PrefabManager.GetInstance().condenser, this.transform.position - new Vector3(0, 0.1f, 0 ), this.transform.rotation);
                break;
            case BuildingType.LIGHTSOURCE:
                building = GameObject.Instantiate(PrefabManager.GetInstance().lightsource, this.transform.position - new Vector3(0, 0.1f, 0 ), this.transform.rotation);
                break;
            case BuildingType.GENERATOR:
                building = GameObject.Instantiate(PrefabManager.GetInstance().generator, this.transform.position - new Vector3(0, 0.1f, 0 ), this.transform.rotation);
                break;
            default:
                break;
        }
    }

    public List<ShopItem> GetShopItems() 
    {
        List<ShopItem> shopItems = new List<ShopItem>();

        GameObject feature = MapManager.GetInstance().FeatureAtCoordinate(this.coordinate);
        if(feature)
        {
            shopItems.Add(ShopItem.MiningPlatform());
            return shopItems;
        }

        switch (this.platformType)
        {
            case PlatformType.PLACEHOLDER:
                shopItems.Add(ShopItem.StonePlatform());
                shopItems.Add(ShopItem.SoilPlatform());
                break;
            case PlatformType.SOIL:
                shopItems.Add(ShopItem.Tree());
                break;
            case PlatformType.STONE:
                shopItems.Add(ShopItem.Condenser());
                shopItems.Add(ShopItem.LightSource());
                shopItems.Add(ShopItem.Generator());
                break;
            default:
                return shopItems;
        }


        return shopItems;
    }
}
