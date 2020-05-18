using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapGenerator : MonoBehaviour
{
    public GameObject hexTilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        GenerateTileMaps();
    }

    void GenerateTileMaps() {
        float hexFlatWidth = 2;
        float hexEdge = hexFlatWidth/(2*Mathf.Sin(Mathf.PI/3));
        float verticalOffset = hexEdge*3/2;
        for(int i=-5; i<6; i++) 
        {
            for(int j=-5; j<6; j++) 
            {
                bool isOddRow = (i % 2 != 0);
                bool isOddColumn = (j % 2 != 0);
                float horizontalOffsetOddRow = isOddRow ? -hexFlatWidth/2 : 0;
                
                Vector3 position = new Vector3(i*verticalOffset, 0, j*2 -horizontalOffsetOddRow);
                GameObject tile = GameObject.Instantiate(hexTilePrefab, position, Quaternion.identity);
                tile.GetComponent<Renderer>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
   
    }
}
