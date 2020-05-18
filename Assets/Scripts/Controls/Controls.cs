using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    HexTile lastHovered;
    void Update()
    {
        Ray ray  = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast (ray, out hit);
        
        HexTile tile = null;
        if(hasHit){
            tile = hit.transform.gameObject.GetComponent<HexTile>();        
        }
        
        if(lastHovered && lastHovered != tile) 
        {
            lastHovered.SetHovered(false);
            lastHovered = null;
        }

        if(tile && lastHovered != tile) 
        {
            tile.SetHovered(true);
            lastHovered = tile;
        }

        if(Input.GetMouseButtonDown(0) && tile)
        {
            tile.Build(TileType.MINERAL);
        }
    }
}
