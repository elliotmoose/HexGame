using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BuildingType {
    TREE
}
public enum PlatformType {
    ROOT,
    PLACEHOLDER,
    MINERAL,
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

    private Material defaultMaterial;
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
        defaultMaterial = GetComponent<Renderer>().material;
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
        
        
        GetComponent<Renderer>().material = isHovered ? hoverMaterial : (isSelected ? selectedMaterial : defaultMaterial);
    }

    public void SetSelected(bool isHovered) {
        if(platformType == PlatformType.NONE) 
        {
            return;
        }
        
        GetComponent<Renderer>().material = isHovered ? hoverMaterial : defaultMaterial;
    }

    public bool CanBuild() {
        return platformType == PlatformType.PLACEHOLDER;
    }

    public void BuildAttachment(AttachmentType attachmentType) {
        if(attachmentType == AttachmentType.HARVESTER && platformType == PlatformType.MINERAL)
        {
            GameObject.Instantiate(PrefabManager.GetInstance().mineralAttachment, this.transform.position - new Vector3(0, 0.1f, 0 ), this.transform.rotation);
        }
    }

    public void BuildBuilding(BuildingType buildingType) {
        if(buildingType == BuildingType.TREE && platformType == PlatformType.SOIL && building == null)  //only can have 1 building per tile
        {
            building = GameObject.Instantiate(PrefabManager.GetInstance().tree, this.transform.position - new Vector3(0, 0.1f, 0 ), this.transform.rotation);
        }
    }

    public List<ShopItem> GetShopItems() 
    {
        List<ShopItem> shopItems = new List<ShopItem>();

        shopItems.Add(new ShopItem("Grass", "A block of grass, let’s you build simple buildings", 500, PrefabManager.GetInstance().grassPlatform));
        shopItems.Add(new ShopItem("Soil", "A block of soil, let's you grow things", 300, PrefabManager.GetInstance().soilPlatform));

        return shopItems;
    }
}
