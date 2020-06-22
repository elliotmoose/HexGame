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

    private Material defaultMaterial;

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

    protected List<HexPlatform> neighbourPlatforms {
        get {
            HexPlatform buildingPlatform = PlatformManager.GetInstance().PlatformAtCoordinate(this.coordinate);
            return PlatformManager.GetInstance().NeighboursOfPlatform(buildingPlatform);
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

    public void SetValidation(bool isOn, bool isValid) {
        if(metaData.id == Identifiers.NULL) 
        {
            return;
        }
        
        Material material = isOn ? (isValid ? Shop.GetInstance().validBuildMaterial : Shop.GetInstance().invalidBuildMaterial) : defaultMaterial;
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

    public virtual void OnBuildUpdate() 
    {

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
