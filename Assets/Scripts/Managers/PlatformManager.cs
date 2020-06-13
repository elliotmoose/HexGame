﻿using System.Collections;
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
    private float _tickRate = 0.5f;
    const int TILEMAP_SIZE = 30;
    // HexPlatform[,] platforms = new HexPlatform[TILEMAP_SIZE, TILEMAP_SIZE];
    Dictionary<Vector2Int, HexPlatform> platforms = new Dictionary<Vector2Int, HexPlatform>();
    
    List<ResourceConsumer> resourceConsumers = new List<ResourceConsumer>();

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
            yield return new WaitForSeconds(1/_tickRate);
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
                var position = Hexagon.PositionForCoordinate(coord, MapManager.HEXAGON_FLAT_WIDTH);
            
                Identifiers platformToSpawn = (isRoot ? Identifiers.SOIL_PLATFORM : Identifiers.EMPTY_PLATFORM);
                GameObject platformGo = _SpawnPlatform(MetaDataManager.GetMetaDataForId(platformToSpawn), coord);
                if(isRoot)
                {
                    GameObject building = BuildBuilding(MetaDataManager.GetMetaDataForId(Identifiers.TREE_BUILDING), coord);
                    building.GetComponent<TreeBuilding>().GrowUp();
                    Shop.GetInstance().selectedPlatform = platformGo.GetComponent<HexPlatform>();                

                }
            }
        }
    }

    private GameObject _SpawnPlatform(ObjectMetaData metaData, Vector2Int coordinate) 
    {        
        HexPlatform platform = PlatformAtCoordinate(coordinate);

        if (platform)
        {
            platforms.Remove(coordinate);
            GameObject.Destroy(platform.gameObject);
        }
        
        var position = Hexagon.PositionForCoordinate(coordinate, MapManager.HEXAGON_FLAT_WIDTH);
        GameObject tileGo = GameObject.Instantiate(metaData.prefab, position, Quaternion.identity, platformParent);
        HexPlatform hexTile = tileGo.GetComponent<HexPlatform>();

        platforms.Add(coordinate, hexTile);
        hexTile.Initialize(metaData, coordinate);
        return tileGo;
    }

    private GameObject BuildPlatform(ObjectMetaData metaData, Vector2Int coordinate)
    {
        HexPlatform platform = PlatformAtCoordinate(coordinate);

        if (platform)
        {
            platforms.Remove(coordinate);
            resourceConsumers.Remove(platform);
            GameObject.Destroy(platform.gameObject);
        }
        
        var position = Hexagon.PositionForCoordinate(coordinate, MapManager.HEXAGON_FLAT_WIDTH);
        GameObject newtile = GameObject.Instantiate(metaData.prefab, position, Quaternion.identity, platformParent);
        HexPlatform hexTile = newtile.GetComponent<HexPlatform>();
        platforms.Add(coordinate, hexTile);
        resourceConsumers.Add(platform);
        hexTile.Initialize(metaData, coordinate);

        UpdatePlaceHolders();
        RecalculateResources();
        return newtile;
    }

    private GameObject BuildBuilding(ObjectMetaData metaData, Vector2Int coordinate)
    {
        HexPlatform platform = PlatformAtCoordinate(coordinate);

        if(platform.building != null)  
        {
            return null;
        }

        GameObject buildingObject = GameObject.Instantiate(metaData.prefab, platform.transform.position, platform.transform.rotation);
        Building building = buildingObject.GetComponent<Building>();
        building.Initialize(metaData, coordinate);

        platform.building = building;
        resourceConsumers.Add(building);
        RecalculateResources();
        return buildingObject;
    }

    public GameObject Build(ObjectMetaData shopItemScriptable, Vector2Int coordinate) 
    {       
        if(shopItemScriptable.type == ShopItemType.BUILDING)
        {
            return BuildBuilding(shopItemScriptable, coordinate);
        }
        else 
        {
            return BuildPlatform(shopItemScriptable, coordinate);
        }
    }

    /// <summary>
    /// we want to update building's effects on each other
    /// </summary>
    public void RecalculateResources() 
    {
        resourceConsumers.Sort((x, y) => x.resourceCalculationOrder.CompareTo(y.resourceCalculationOrder));
        foreach(var consumer in resourceConsumers) 
        {
            consumer.ResetResources();
        }

        //GLOBAL RESOURCE DISTRIBUTION FIRST
        foreach(var consumer in resourceConsumers) 
        {
            Generator generator = consumer as Generator;
            if(generator != null)
            {
                if(Player.GetInstance().oil != 0)
                {
                    generator.ReceiveResource(ResourceIdentifiers.OIL, 1);
                }
            }
        }

        foreach(var consumer in resourceConsumers) 
        {
            consumer.RecalculateResources();
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
                if (neighbour.metaData.id == Identifiers.EMPTY_PLATFORM && (tile.metaData.id != Identifiers.EMPTY_PLATFORM && tile.metaData.id != Identifiers.PLACEHOLDER_PLATFORM))
                {
                    newPlaceholders.Add(neighbour);
                }
            }
        }

        foreach (var tile in newPlaceholders)
        {
            _SpawnPlatform(MetaDataManager.GetMetaDataForId(Identifiers.PLACEHOLDER_PLATFORM), tile.coordinate);
        }
    }

    public List<HexPlatform> NeighboursOfPlatform(HexPlatform platform)
    {
        List<HexPlatform> neighbours = new List<HexPlatform>();
        List<Vector2Int> neighbourCoordinates = Hexagon.NeighbourCoordinatesOfHexagon(platform.coordinate);

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

    public List<HexPlatform> NeighboursOfPlatformWithAxis(HexPlatform platform, int axis)
    {
        List<HexPlatform> neighbours = new List<HexPlatform>();
        List<Vector2Int> neighbourCoordinates = Hexagon.NeighbourCoordinatesOfHexagonWithAxis(platform.coordinate, axis);

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