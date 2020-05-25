using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public void BuildPlatform(PlatformType platformType, Vector2Int coord)
    {
        if (PlatformManager.GetInstance().BuildPlatform(platformType, coord))
        {
            Player.GetInstance().TransactMinerals(-100);
        }
    }

    public void BuildAttachment(AttachmentType attachmentType, Vector2Int coord) 
    {
        HexPlatform tile = PlatformManager.GetInstance().HexTileAtCoordinate(coord);
        tile.BuildAttachment(AttachmentType.HARVESTER);
    }

    public void BuildBuilding(BuildingType buildingType, Vector2Int coord) 
    {
        HexPlatform tile = PlatformManager.GetInstance().HexTileAtCoordinate(coord);
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
