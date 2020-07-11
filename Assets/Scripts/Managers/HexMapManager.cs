using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMapManager : MonoBehaviour
{
    public GameObject tile;
    const int CHUNK_WIDTH = 4;
    const int loadAdjacentChunkThreshold = 6; 

    public float PERLIN_SCALE = 10;
    public float HEIGHT_SCALE = 4;
    public float HEIGHT_CAP = 4;
    public float HEIGHT_FLOOR = 0.5f;

    public float MOISTURE_THRESHOLD = 0.3f;
    public float WATER_THRESHOLD = 0.3f;
    public float SOIL_THRESHOLD = 0.65f;
    public float SAND_THRESHOLD = 0.65f;
    public float STONE_THRESHOLD = 1;

    public float islandSize = CHUNK_WIDTH * (loadAdjacentChunkThreshold * 2 + 1);

    public Color waterColor;
    public Color stoneColor;
    public Color soilColor;
    public Color sandColor;
    public Color metalColor;
    public Color copperColor;
    
    private Vector2Int currentChunk = new Vector2Int(0,0);

    private Vector2 heightPerlinOffset = Vector2.zero;
    private Vector2 moistPerlinOffset = Vector2.zero;

    public GameObject mapParent;

    public Dictionary<Vector2Int, GameObject> tiles = new Dictionary<Vector2Int, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GeneratePerlinOffset();
        GenerateMap();
        // UpdateInBaseHexTiles();
    }
    
    void Update()
    {
        // UpdateHexHeights();
    }

    void GeneratePerlinOffset() 
    {
        float start = 1000 * PERLIN_SCALE;        
        heightPerlinOffset = new Vector2(Random.Range(start, start*10), Random.Range(start, start*10));
        moistPerlinOffset = new Vector2(Random.Range(start, start*10), Random.Range(start, start*10));
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

        // UpdateHexHeights();
    }

    void UpdateInBaseHexTiles()
    {                   
        foreach(var tile in tiles.Values)
        {
            HexTile platform = tile.GetComponent<HexTile>();
            platform.inBase = false;
            // int range = 3;
            // if((platform.coordinate - Vector2Int.zero).magnitude < range)
            // {
            //     platform.inBase = true;
            // }
            // else 
            // {
            //     platform.inBase = false;
            // }
        }
        
        Base.GetInstance().UpdateInBaseTiles();

        // foreach(var tile in tiles.Values)
        // {
        //     HexPlatform platform = tile.GetComponent<HexPlatform>();
        //     platform.UpdateBorder();   
        // }
    }

    void GenerateChunk(Vector2Int chunkCoordinate)
    {
        for(int i=-CHUNK_WIDTH/2; i < CHUNK_WIDTH/2; i++)
        {
            for(int j=-CHUNK_WIDTH/2; j < CHUNK_WIDTH/2; j++)
            {
                Vector2Int relativeCoordinate = new Vector2Int(i, j);
                Vector2Int objectiveCoordinate = relativeCoordinate + chunkCoordinate*CHUNK_WIDTH;
                GenerateChunkUnit(objectiveCoordinate);                
            }
        }
    }
    
    void UpdateHexHeights()
    {
        for (int i = -loadAdjacentChunkThreshold; i <= loadAdjacentChunkThreshold; i++)
        {
            for (int j = -loadAdjacentChunkThreshold; j <= loadAdjacentChunkThreshold; j++)
            {
                Vector2Int chunkCoordinate = new Vector2Int(currentChunk.x + i, currentChunk.y + j);
                for(int x=-CHUNK_WIDTH/2; x < CHUNK_WIDTH/2; x++)
                {
                    for(int y=-CHUNK_WIDTH/2; y < CHUNK_WIDTH/2; y++)
                    {                        
                        Vector2Int relativeCoordinate = new Vector2Int(x, y);
                        Vector2Int coordinate = relativeCoordinate + chunkCoordinate*CHUNK_WIDTH;
                        GameObject tile = tiles[coordinate];
                        

                        //position and height
                        Vector3 position = Hexagon.PositionForCoordinate(coordinate);
                        float height = HeightMap(coordinate);
                        // float moisture = MoistMap(coordinate);
                        Vector3 heightAdjustedPos = new Vector3(position.x, mapParent.transform.position.y + height/2, position.z);
                        tile.transform.localScale = new Vector3(1, height, 1);
                        tile.transform.position = heightAdjustedPos;

                        //material and tile
                        Color color = stoneColor;
                        
                        if(height/HEIGHT_CAP < WATER_THRESHOLD)
                        {
                            color = waterColor;
                        }
                        else if(height/HEIGHT_CAP < SAND_THRESHOLD)
                        {
                            color = sandColor;
                        }
                        else if(height/HEIGHT_CAP < SOIL_THRESHOLD)
                        {
                            color = soilColor;
                            
                            Random.InitState(coordinate.x + coordinate.y);
                            bool isMetal = (Random.value < 0.1f);
                            bool isCopper = (Random.value < 0.2f);

                            if(isMetal) 
                            {
                                color = metalColor;
                            }
                            else if (isCopper)
                            {
                                color = copperColor;
                            }
                        }
                        else if(height/HEIGHT_CAP < STONE_THRESHOLD)
                        {
                            color = stoneColor;
                        
                        }

                        tile.GetComponent<Renderer>().material.color = color;
                    }
                }
            }
        }
        
    }

    void GenerateChunkUnit(Vector2Int coordinate) 
    {
        //root is reserved for tree
        Vector3 position = Hexagon.PositionForCoordinate(coordinate);
        
        //HEIGHT MAP: ALL COMES FROM HERE!!
        float height = HeightMap(coordinate);

        Vector3 heightAdjustedPos = new Vector3(position.x, mapParent.transform.position.y + height/2, position.z);
        GameObject tileGameObject = GameObject.Instantiate(this.tile, heightAdjustedPos, Quaternion.identity, mapParent.transform);            
        tiles.Add(coordinate, tileGameObject);
        HexTile tile = tileGameObject.GetComponent<HexTile>();        
        tileGameObject.transform.localScale = new Vector3(1, height, 1);
        tileGameObject.transform.position = heightAdjustedPos;

        //material and tile
        Color color = stoneColor;
        
        bool isWater = (height/HEIGHT_CAP < WATER_THRESHOLD);
        bool isSand = (height/HEIGHT_CAP < SAND_THRESHOLD) && !isWater;
        bool isSoil = (height/HEIGHT_CAP < SOIL_THRESHOLD) && !isSand && !isWater;
        bool isStone = (height/HEIGHT_CAP < STONE_THRESHOLD) && !isSoil && !isSand && !isWater;
        bool isMetal = (isSoil || isStone) && (Random.Range(0, 1.0f) < 0.03f);
        bool isCopper = (isSoil || isStone) && (Random.Range(0,1.0f) < 0.02f) && !isMetal;
        if(isWater)
        {
            tile.Initialize(TileIdentifiers.WATER, coordinate);
            color = waterColor;
        }
        else if(isSand)
        {
            tile.Initialize(TileIdentifiers.SAND, coordinate);
            color = sandColor;
        }
        else if(isSoil && !(isMetal || isCopper))
        {
            tile.Initialize(TileIdentifiers.SOIL, coordinate);
            color = soilColor;
        }
        else if(isStone && !(isMetal || isCopper))
        {
            tile.Initialize(TileIdentifiers.STONE, coordinate);
            color = stoneColor;        
        }
        else if(isMetal) 
        {
            tile.Initialize(TileIdentifiers.METAL, coordinate);
            color = metalColor;
        }
        else if (isCopper)
        {
            tile.Initialize(TileIdentifiers.COPPER, coordinate);
            color = copperColor;
        }

        tileGameObject.GetComponent<Renderer>().material.color = color;
    }

    public GameObject TileAtCoordinate(Vector2Int coord) 
    {
        GameObject output;
        tiles.TryGetValue(coord, out output);
        return output;
    }

    public float HeightMap(Vector2Int coordinate)
    {
        float perlin = Perlin(coordinate, heightPerlinOffset, PERLIN_SCALE, HEIGHT_SCALE);
        float island = QuadraticDist(coordinate, 0);        
        return Mathf.Clamp(perlin * island, HEIGHT_FLOOR, HEIGHT_CAP);
    }
    
    float MoistMap(Vector2Int coordinate)
    {
        float perlin = Perlin(coordinate, moistPerlinOffset, PERLIN_SCALE, 1);
        float island = 1-QuadraticDist(coordinate, 12);        
        return Mathf.Clamp(perlin*island, 0, 1);
    }

    float Perlin(Vector2Int coordinate, Vector2 offset, float perlinScale, float heightScale)
    {
        return Mathf.PerlinNoise(offset.x + coordinate.x/perlinScale, offset.y + coordinate.y/perlinScale) * heightScale;
    }
    
    float QuadraticDist(Vector2Int coordinate, float padding)
    {        
        float distance_x = Mathf.Abs(coordinate.x);
        float distance_y = Mathf.Abs(coordinate.y);
        float distance = Mathf.Sqrt(distance_x*distance_x + distance_y*distance_y); // circular mask

        float maxDistance = islandSize * 0.5f - padding;
        float delta = distance / maxDistance;
        float gradient = delta * delta * delta;

        return Mathf.Max(0.0f, 1.0f - gradient);
    }

    
    private static HexMapManager _singleton;

    HexMapManager() {
        _singleton = this;
    }

    public static HexMapManager GetInstance() 
    {
        return _singleton;
    }
}
