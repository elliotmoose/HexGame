using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    HexPlatform lastHovered;
    AttachmentType selectedAttachmentType = AttachmentType.HARVESTER;

    void Update()
    {
        Ray ray  = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast (ray, out hit);
        
        HexPlatform tile = null;
        if(hasHit)
        {            
            tile = hit.transform.gameObject.GetComponent<HexPlatform>();        
            
            if(Input.GetMouseButtonDown(0) && tile)
            {
                var coord = tile.coordinate;
                if(tile.tileType == PlatformType.PLACEHOLDER) 
                {
                    Shop.GetInstance().BuildPlatform(PlatformType.SOIL, tile.coordinate);                    
                }
                else if (tile.tileType == PlatformType.MINERAL) 
                {                    
                    Shop.GetInstance().BuildAttachment(AttachmentType.HARVESTER, tile.coordinate);                                        
                }
                else if (tile.tileType == PlatformType.SOIL) 
                {                    
                    Shop.GetInstance().BuildBuilding(BuildingType.TREE, tile.coordinate);                                        
                }
            }

            HexPlatform lastLastHovered = null;
            if(lastHovered != tile && tile != null) 
            {
                lastLastHovered = lastHovered;
                tile.SetHovered(true);
                lastHovered = tile;
            
                if(lastLastHovered) 
                {
                    lastLastHovered.SetHovered(false);                                
                }            
            }

        }
        else if (lastHovered)
        {
            lastHovered.SetHovered(false);
            lastHovered = null;
        }        
    }
}
