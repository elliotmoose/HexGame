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
    public PlatformType tileType = PlatformType.NONE;
    public Vector2Int coordinate;

    private Material defaultMaterial;
    public Material hoverMaterial;

    public GameObject building;

    // Start is called before the first frame update
    void Start()
    {
        defaultMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(PlatformType tileType, Vector2Int coord) {
        this.tileType = tileType;
        coordinate = coord;
    }

    public void SetHovered(bool isHovered) {
        if(tileType == PlatformType.NONE) 
        {
            return;
        }
        
        GetComponent<Renderer>().material = isHovered ? hoverMaterial : defaultMaterial;
    }

    public bool CanBuild() {
        return tileType == PlatformType.PLACEHOLDER;
    }

    public void BuildAttachment(AttachmentType attachmentType) {
        if(attachmentType == AttachmentType.HARVESTER && tileType == PlatformType.MINERAL)
        {
            GameObject.Instantiate(PrefabManager.GetInstance().mineralAttachment, this.transform.position - new Vector3(0, 0.1f, 0 ), this.transform.rotation);
        }
    }

    public void BuildBuilding(BuildingType buildingType) {
        if(buildingType == BuildingType.TREE && tileType == PlatformType.SOIL && building == null)  //only can have 1 building per tile
        {
            building = GameObject.Instantiate(PrefabManager.GetInstance().tree, this.transform.position - new Vector3(0, 0.1f, 0 ), this.transform.rotation);
        }
    }
}
