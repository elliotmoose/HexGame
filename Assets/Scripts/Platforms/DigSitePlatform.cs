using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigSitePlatform : HexPlatform
{
    public List<GameObject> walls = new List<GameObject>();

    public override void OnBuildUpdate()
    {
        for(int i=0; i<6;i++)
        {
            Vector2Int coord = Hexagon.NeighbourAtIndex(this.coordinate, i);
            HexPlatform platform = PlatformManager.GetInstance().PlatformAtCoordinate(coord);
            Debug.Log(platform.metaData.id);
            
            if(platform.metaData.id == Identifiers.DIG_SITE_PLATFORM)
            {
                walls[i].SetActive(false);
            }
        }
    }
}
