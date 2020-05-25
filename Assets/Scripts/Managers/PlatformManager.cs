using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject hexPlatformPrefab;
    public GameObject soilPlatformPrefab;
    public GameObject mineralPlatformPrefab;
    public GameObject emptyPlatformPrefab;
    public GameObject placeholderPlatformPrefab;
    public Transform platformParent;

    const int TILEMAP_SIZE = 30;
    // HexPlatform[,] platforms = new HexPlatform[TILEMAP_SIZE, TILEMAP_SIZE];
    Dictionary<Vector2Int, HexPlatform> platforms = new Dictionary<Vector2Int, HexPlatform>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateTileMaps();
        UpdatePlaceHolders();
    }

    void GenerateTileMaps()
    {           
        // Vector2Int rootTileCoordinate = new Vector2Int((int)TILEMAP_SIZE / 2, (int)TILEMAP_SIZE / 2);
        Vector2Int rootTileCoordinate = Vector2Int.zero;

        for (int i = -TILEMAP_SIZE/2; i <= TILEMAP_SIZE/2; i++)
        {
            for (int j = -TILEMAP_SIZE/2; j <= TILEMAP_SIZE/2; j++)
            {
                var coord = new Vector2Int(i, j);
                var isRoot = (rootTileCoordinate == coord);
                HexPlatform tile = SpawnPlatform(isRoot ? PlatformType.SOIL : PlatformType.NONE, coord);

                if(isRoot) {
                    Camera.main.transform.position = new Vector3(tile.transform.position.x + 5, 10, tile.transform.position.z);
                    Color rootColor;
                    ColorUtility.TryParseHtmlString("#4244B7", out rootColor);
                    tile.GetComponent<Renderer>().material.color = rootColor;
                }
            }
        }
    }

    /// <summary>
    /// Creates or replaces a tile at the coordinate no questions asked
    /// </summary>
    /// <param name="buildTileType"></param>
    /// <param name="coordinate"></param>
    /// <returns></returns>
    private HexPlatform SpawnPlatform(PlatformType buildTileType, Vector2Int coordinate) 
    {        
        HexPlatform platform = HexTileAtCoordinate(coordinate);

        if (platform)
        {
            platforms.Remove(coordinate);
            GameObject.Destroy(platform.gameObject);
        }
        
        GameObject prefab = null;
        var position = Hexagon.PositionOfHexagonAtCoordinate(coordinate, MapManager.HEXAGON_FLAT_WIDTH);

        switch (buildTileType)
        {
            case PlatformType.PLACEHOLDER:
                prefab = placeholderPlatformPrefab;
                // GetComponent<Renderer>().material.color = deselectedColor;
                break;
            case PlatformType.SOIL:
                prefab = soilPlatformPrefab;
                break;
            
            case PlatformType.MINERAL:
                prefab = mineralPlatformPrefab;
                break;

            default:
                prefab = emptyPlatformPrefab;
                break;
        }

        GameObject tileGo = GameObject.Instantiate(prefab, position, Quaternion.identity, platformParent);
        HexPlatform hexTile = tileGo.GetComponent<HexPlatform>();

        platforms.Add(coordinate, hexTile);
        hexTile.Initialize(buildTileType, coordinate);
        return hexTile;
    }

    /// <summary>
    /// Builds a tile at the location if possible, and replaces previous tiles
    /// </summary>
    /// <param name="buildTileType"></param>
    /// <param name="coordinate"></param>
    /// <returns></returns>
    public HexPlatform BuildPlatform(PlatformType buildTileType, Vector2Int coordinate)
    {
        HexPlatform tile = HexTileAtCoordinate(coordinate);

        if(!tile.CanBuild()) 
        {
            return null;
        }

        if (tile)
        {
            GameObject.Destroy(tile.gameObject);
        }

        HexPlatform newtile = SpawnPlatform(buildTileType, coordinate);
        UpdatePlaceHolders();
        return newtile;
    }

    public void UpdatePlaceHolders()
    {
        List<HexPlatform> newPlaceholders = new List<HexPlatform>();
        foreach (HexPlatform tile in platforms.Values)
        {
            List<HexPlatform> neighbours = NeighboursOfTile(tile);

            foreach (HexPlatform neighbour in neighbours)
            {
                if (neighbour.tileType == PlatformType.NONE && (tile.tileType != PlatformType.NONE && tile.tileType != PlatformType.PLACEHOLDER))
                {
                    newPlaceholders.Add(neighbour);
                }
            }
        }

        foreach (var tile in newPlaceholders)
        {
            SpawnPlatform(PlatformType.PLACEHOLDER, tile.coordinate);
        }
    }

    List<HexPlatform> NeighboursOfTile(HexPlatform tile)
    {
        List<HexPlatform> neighbours = new List<HexPlatform>();

        Vector2Int refCoord = tile.coordinate;
        var isOdd = ((tile.coordinate.x % 2) == 0);
        int[][] neighbourOffsets = {
            new int[]{0, 1}, 
            new int[]{0, -1},
            new int[]{1, 0},
            new int[]{1, isOdd ? -1 : 1},
            new int[]{-1, 0},
            new int[]{-1, isOdd ? -1 : 1},
        };

        foreach (int[] offset in neighbourOffsets)
        {
            HexPlatform neighbour = HexTileAtCoordinate(new Vector2Int(refCoord.x + offset[0], refCoord.y + offset[1]));            
            if(neighbour) 
            {
                neighbours.Add(neighbour);
            }
        }

        return neighbours;
    }

    public HexPlatform HexTileAtCoordinate(Vector2Int coordinate)
    {
        //validation
        HexPlatform tile;
        platforms.TryGetValue(coordinate, out tile);
        return tile;
    }

    private static PlatformManager _singleton;

    PlatformManager() {
        _singleton = this;
    }

    public static PlatformManager GetInstance() 
    {
        return _singleton;
    }
}
