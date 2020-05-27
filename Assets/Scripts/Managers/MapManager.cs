using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static float HEXAGON_FLAT_WIDTH = 2;
    private static int CHUNK_WIDTH = 4;
    private Vector2Int currentChunk = new Vector2Int(0,0);
    private int loadAdjacentChunkThreshold = 1; 

    public GameObject mapParent;
    public GameObject mineralPrefab;

    Dictionary<Vector2Int, GameObject> mapFeatures = new Dictionary<Vector2Int, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int i = -loadAdjacentChunkThreshold; i <= loadAdjacentChunkThreshold; i++)
        {
            for (int j = -loadAdjacentChunkThreshold; j <= loadAdjacentChunkThreshold; j++)
            {
                GenerateChunk(new Vector2Int(currentChunk.x + i, currentChunk.y + j));
            }
        }
    }

    void GenerateChunk(Vector2Int chunkCoordinate)
    {
        for(int i=-MapManager.CHUNK_WIDTH/2; i < MapManager.CHUNK_WIDTH/2; i++)
        {
            for(int j=-MapManager.CHUNK_WIDTH/2; j < MapManager.CHUNK_WIDTH/2; j++)
            {
                Vector2Int relativeCoordinate = new Vector2Int(i, j);
                Vector2Int objectiveCoordinate = relativeCoordinate + chunkCoordinate*CHUNK_WIDTH;
                GenerateChunkUnit(objectiveCoordinate);
            }
        }
    }

    void GenerateChunkUnit(Vector2Int coordinate) 
    {
        var randInt =  Random.Range(0, 10);
        if(randInt < 1) 
        {
            Vector3 position = Hexagon.PositionForCoordinate(coordinate, MapManager.HEXAGON_FLAT_WIDTH);
            Vector3 heightAdjustedPos = new Vector3(position.x, mapParent.transform.position.y, position.z);
            GameObject feature = GameObject.Instantiate(mineralPrefab, heightAdjustedPos, Quaternion.identity, mapParent.transform);            
            mapFeatures.Add(coordinate, feature);
        }
    }

        
    private static MapManager _singleton;

    MapManager() {
        _singleton = this;
    }

    public static MapManager GetInstance() 
    {
        return _singleton;
    }
}
