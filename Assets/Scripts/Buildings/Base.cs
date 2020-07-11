using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : Building
{
    // Start is called before the first frame update
    void Start()
    {
        UpdateInBaseTiles();
    }

    public void UpdateInBaseTiles() 
    {
        foreach(var tile in HexMapManager.GetInstance().tiles.Values)
        {
            HexTile platform = tile.GetComponent<HexTile>();
            platform.inBase = false;
        }

        float range = 3.5f;
        float verticalHeight = HexMapManager.GetInstance().HEIGHT_CAP;
        Vector3 capsuleOffset = new Vector3(0,verticalHeight + range,0);
        Collider[] colliders = Physics.OverlapCapsule(this.transform.position + capsuleOffset, this.transform.position - capsuleOffset, range);
        foreach(var collider in colliders)
        {
            HexTile platform = collider.GetComponent<HexTile>();
            if(platform != null)
            {
                platform.inBase = true;                
            }
        }
        
        foreach(var tile in HexMapManager.GetInstance().tiles.Values)
        {
            HexTile platform = tile.GetComponent<HexTile>();
            platform.UpdateBorder();
        }
    }

        
    private static Base _singleton = null;

    Base() {
        _singleton = this;
    }

    public static Base GetInstance() 
    {
        return _singleton;
    }
}
