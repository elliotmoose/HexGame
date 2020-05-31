using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AttachmentType {
    HARVESTER
}

public class HexPlatform : MonoBehaviour
{
    public Identifiers id = Identifiers.NULL;
    public Vector2Int coordinate;

    public Material defaultMaterial;
    public Material hoverMaterial;
    public Material selectedMaterial;

    private GameObject _building;
    public GameObject building {
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
            defaultMaterial = GetComponent<Renderer>().material;
        }
    }

    public void Initialize(Identifiers tileType, Vector2Int coord) {
        this.id = tileType;
        coordinate = coord;
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

    // public void BuildAttachment(AttachmentType attachmentType) {
    //     if(attachmentType == AttachmentType.HARVESTER && platformType == PlatformType.MINERAL)
    //     {
    //         GameObject.Instantiate(PrefabManager.GetInstance().mineralAttachment, this.transform.position - new Vector3(0, 0.1f, 0 ), this.transform.rotation);
    //     }
    // }


    public List<ShopItem> GetShopItems() 
    {
        List<ShopItem> shopItems = new List<ShopItem>();

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
                break;
            case Identifiers.SOIL_PLATFORM:
                shopItems.Add(ShopItem.Tree());
                break;
            case Identifiers.STONE_PLATFORM:
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
