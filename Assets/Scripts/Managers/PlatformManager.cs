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
        StartCoroutine(Tick());
    }

    IEnumerator Tick() 
    {
        while(true)
        {
            foreach (HexPlatform platform in platforms.Values)
            {
                platform.Tick();
            }
            yield return new WaitForSeconds(0.5f);
        }
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
                GameObject tile = _SpawnPlatform(isRoot ? Identifiers.SOIL_PLATFORM : Identifiers.EMPTY_PLATFORM, coord);

                if(isRoot) {
                    Camera.main.transform.position = new Vector3(tile.transform.position.x + 5, 10, tile.transform.position.z);
                    Color rootColor;
                    ColorUtility.TryParseHtmlString("#4244B7", out rootColor);
                    tile.GetComponent<Renderer>().material.color = rootColor;
                }
            }
        }
    }

    private GameObject _SpawnPlatform(Identifiers id, Vector2Int coordinate) 
    {        
        HexPlatform platform = PlatformAtCoordinate(coordinate);

        if (platform)
        {
            platforms.Remove(coordinate);
            GameObject.Destroy(platform.gameObject);
        }
        
        var position = Hexagon.PositionForCoordinate(coordinate, MapManager.HEXAGON_FLAT_WIDTH);
        GameObject tileGo = GameObject.Instantiate(PrefabManager.PrefabForID(id), position, Quaternion.identity, platformParent);
        HexPlatform hexTile = tileGo.GetComponent<HexPlatform>();

        platforms.Add(coordinate, hexTile);
        hexTile.Initialize(id, coordinate);
        return tileGo;
    }

    private GameObject BuildPlatform(Identifiers platformType, Vector2Int coordinate)
    {
        HexPlatform platform = PlatformAtCoordinate(coordinate);

        if (platform)
        {
            GameObject.Destroy(platform.gameObject);
        }

        GameObject newtile = _SpawnPlatform(platformType, coordinate);
        UpdatePlaceHolders();
        OnSystemUpdate();
        return newtile;
    }

    private GameObject BuildBuilding(Identifiers id, Vector2Int coordinate)
    {
        HexPlatform platform = PlatformAtCoordinate(coordinate);

        if(platform.building != null)  
        {
            return null;
        }

        GameObject buildingObject = GameObject.Instantiate(PrefabManager.PrefabForID(id), platform.transform.position, platform.transform.rotation);
        Building building = buildingObject.GetComponent<Building>();
        building.Initialize(id, coordinate);

        platform.building = building;
        OnSystemUpdate();
        return buildingObject;
    }


    public bool CanBuild(Identifiers id, Vector2Int coordinate) 
    {
        HexPlatform targetPlatform = PlatformAtCoordinate(coordinate);
        Identifiers platformId = targetPlatform.id;
        bool isPlaceholder = targetPlatform.id == Identifiers.PLACEHOLDER_PLATFORM;
        switch (id)
        {
            //platforms
            case Identifiers.STONE_PLATFORM:
            case Identifiers.SOIL_PLATFORM:
            case Identifiers.MINING_PLATFORM:
                return isPlaceholder;
            //buildings
            case Identifiers.TREE_BUILDING:
                return platformId == Identifiers.SOIL_PLATFORM;
            case Identifiers.CONDENSER_BUILDING:
            case Identifiers.LIGHTSOURCE_BUILDING:
            case Identifiers.GENERATOR_BUILDING:
            case Identifiers.TURBINE_BUILDING:
                return platformId == Identifiers.STONE_PLATFORM;
            default:
                return false;
        }
    }

    public GameObject Build(Identifiers id , Vector2Int coordinate) 
    {
        if(!CanBuild(id, coordinate)) 
        {
            Debug.LogWarning($"Cannot build {id} at {coordinate}");
            return null;
        }
        
        switch (id)
        {
            case Identifiers.STONE_PLATFORM:
            case Identifiers.SOIL_PLATFORM:
            case Identifiers.MINING_PLATFORM:
                return BuildPlatform(id, coordinate);
            case Identifiers.TREE_BUILDING:
            case Identifiers.LIGHTSOURCE_BUILDING:
            case Identifiers.CONDENSER_BUILDING:
            case Identifiers.GENERATOR_BUILDING:
            case Identifiers.TURBINE_BUILDING:
                return BuildBuilding(id, coordinate);
            default:
                return null;
        }
    }

    /// <summary>
    /// we want to update building's effects on each other
    /// </summary>
    public void OnSystemUpdate() 
    {
        //TODO: update only nearby buildings?
        foreach(HexPlatform platform in platforms.Values) 
        {
            if(platform.id != Identifiers.EMPTY_PLATFORM)
            {
                platform.OnSystemUpdate();
            }
        }
    }

    public void UpdatePlaceHolders()
    {
        List<HexPlatform> newPlaceholders = new List<HexPlatform>();
        foreach (HexPlatform tile in platforms.Values)
        {
            List<HexPlatform> neighbours = NeighboursOfPlatform(tile);

            foreach (HexPlatform neighbour in neighbours)
            {
                if (neighbour.id == Identifiers.EMPTY_PLATFORM && (tile.id != Identifiers.EMPTY_PLATFORM && tile.id != Identifiers.PLACEHOLDER_PLATFORM))
                {
                    newPlaceholders.Add(neighbour);
                }
            }
        }

        foreach (var tile in newPlaceholders)
        {
            _SpawnPlatform(Identifiers.PLACEHOLDER_PLATFORM, tile.coordinate);
        }
    }

    public List<HexPlatform> NeighboursOfPlatform(HexPlatform platform)
    {
        List<HexPlatform> neighbours = new List<HexPlatform>();
        List<Vector2Int> neighbourCoordinates = Hexagon.NeightbourCoordinatesOfHexagon(platform.coordinate);

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


/*
Adding a new block
1. CanBuild (PlatformManager)
2. Build (PlatformManager)
3. BuildPlatform/BuildBuilding (PlatformManager)
4. PrefabForID (PrefabManager)
5. Add prefab (PrefabManager)
6. Create Shop Item (ShopItem)
7. Create Identifier (Identifiers)
8. GetShopItems (HexPlatform)
*/