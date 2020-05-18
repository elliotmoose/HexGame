using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TileType {
    PLACEHOLDER,
    MINERAL,
}
public class HexTile : MonoBehaviour
{
    TileType tileType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Build(TileType buildTileType) 
    {
        this.tileType = buildTileType;
        GetComponent<Renderer>().enabled = true;
    }

    public void SetHovered(bool isHovered) {
        
        if(tileType == TileType.PLACEHOLDER) {
            GetComponent<Renderer>().enabled = isHovered;
        }
        else {
            Color selectedColor;
            Color deselectedColor;
            ColorUtility.TryParseHtmlString("#B79642", out selectedColor);
            ColorUtility.TryParseHtmlString("#43B741", out deselectedColor);
            GetComponent<Renderer>().material.color = isHovered ? selectedColor : deselectedColor;
        }
    }

    
}
