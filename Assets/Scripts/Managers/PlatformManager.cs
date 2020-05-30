using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    // public GameObject hexPlatformPrefab;
    // public GameObject soilPlatformPrefab;
    // public GameObject mineralPlatformPrefab;
    // public GameObject emptyPlatformPrefab;
    // public GameObject placeholderPlatformPrefab;
    public Transform platformParent;

    const int TILEMAP_SIZE = 30;
    // HexPlatform[,] platforms = new HexPlatform[TILEMAP_SIZE, TILEMAP_SIZE];
    Dictionary<Vector2Int, HexPlatform> platforms = new Dictionary<Vector2Int, HexPlatform>();

    // Start is called before the first frame update
    void Start()
    {
        GeneratePlatformMap();
        UpdatePlaceHolders();
    }

    void GeneratePlatformMap()
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
        HexPlatform platform = PlatformAtCoordinate(coordinate);

        if (platform)
        {
            platforms.Remove(coordinate);
            GameObject.Destroy(platform.gameObject);
        }
        
        GameObject prefab = null;
        var position = Hexagon.PositionForCoordinate(coordinate, MapManager.HEXAGON_FLAT_WIDTH);

        switch (buildTileType)
        {
            case PlatformType.PLACEHOLDER:
                prefab = PrefabManager.GetInstance().placeholderPlatform;
                // GetComponent<Renderer>().material.color = deselectedColor;
                break;
            case PlatformType.STONE:
                prefab = PrefabManager.GetInstance().stonePlatform;
                break;
            case PlatformType.SOIL:
                prefab = PrefabManager.GetInstance().soilPlatform;
                break;
            
            case PlatformType.MINING:
                prefab = PrefabManager.GetInstance().miningPlatform;
                break;

            default:
                prefab = PrefabManager.GetInstance().emptyPlatform;
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
        HexPlatform tile = PlatformAtCoordinate(coordinate);

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
                if (neighbour.platformType == PlatformType.NONE && (tile.platformType != PlatformType.NONE && tile.platformType != PlatformType.PLACEHOLDER))
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
        List<Vector2Int> neighbourCoordinates = Hexagon.NeightbourCoordinatesOfHexagon(tile.coordinate);

        foreach (Vector2Int coordinate in neighbourCoordinates)
        {
            HexPlatform neighbour = PlatformAtCoordinate(coordinate);            
            if(neighbour) 
            {
                neighbours.Add(neighbour);
            }
        }

        return neighbours;
    }

    public HexPlatform PlatformAtCoordinate(Vector2Int coordinate)
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
