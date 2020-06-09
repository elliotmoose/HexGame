using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : ResourceConsumer
{
    public Identifiers id = Identifiers.NULL;
    public Vector2Int coordinate = Vector2Int.zero;

    protected List<HexPlatform> neighbourPlatforms {
        get {
            HexPlatform buildingPlatform = PlatformManager.GetInstance().PlatformAtCoordinate(this.coordinate);
            return PlatformManager.GetInstance().NeighboursOfPlatform(buildingPlatform);
        }
    }

    protected List<Building> neighbourBuildings {
        get {
            HexPlatform buildingPlatform = PlatformManager.GetInstance().PlatformAtCoordinate(this.coordinate);
            List<HexPlatform> platforms = PlatformManager.GetInstance().NeighboursOfPlatform(buildingPlatform);
            List<Building> neighbours = new List<Building>();

            foreach (HexPlatform platformNeighbour in platforms)
            {
                if(platformNeighbour.building != null)
                {
                    neighbours.Add(platformNeighbour.building);
                }
            }

            return neighbours;
        }
    }

    public void Initialize(Identifiers id, Vector2Int coord) 
    {
        this.id = id;
        coordinate = coord;
        InitializeResourceNeeds();
    }

    protected virtual void Update() 
    {
        UpdateIndicatorsPosition();
    }

    public virtual void BuildingTick() {}

    public virtual void OnSystemUpdateBuilding() {}

    public virtual string GetDescription() {return"";}
}
