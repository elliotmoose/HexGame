using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AttachmentType {
    HARVESTER
}

public class HexPlatform : ResourceConsumer
{
    public ObjectMetaData metaData;
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

    public void Initialize(ObjectMetaData metaData, Vector2Int coord) {
        this.metaData = metaData;
        coordinate = coord;
        resourceCalculationOrder = metaData.resourceRecalculationOrder;
        InitializeResourceNeeds();
    }

    public void SetHovered(bool isHovered) {
        if(metaData.id == Identifiers.NULL) 
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
        if(metaData.id == Identifiers.NULL) 
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

    protected virtual void PlatformTick() 
    {
        
    }

    public virtual void Reselect() 
    {
        if(building)
        {
            building.Reselect();
        }
    }

    public virtual string GetDescription()
    {
        string description = $"{metaData.id}\n";
        if(building)
        {
            description += building.GetDescription();
        }
        return description;
    }
}
