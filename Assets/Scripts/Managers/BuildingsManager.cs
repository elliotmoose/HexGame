using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsManager : MonoBehaviour
{
    private float _tickRate = 0.5f;
    Dictionary<Vector2Int, Building> buildings = new Dictionary<Vector2Int, Building>();    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Tick());
    }

    IEnumerator Tick() 
    {
        while(true)
        {
            foreach (Building building in buildings.Values)
            {
                building.BuildingTick();
            }
            yield return new WaitForSeconds(1/_tickRate);
        }
    }
    
    private GameObject BuildBuilding(ObjectMetaData metaData, Vector2Int coordinate)
    {
        GameObject tile = HexMapManager.GetInstance().TileAtCoordinate(coordinate);

        bool isUpgrade = false;
        
        Building oldBuilding = BuildingAtCoordinate(coordinate);
        if(oldBuilding != null)  
        {
            //check upgrade
            if(oldBuilding.metaData.id == metaData.id)
            {
                //is upgrade
                isUpgrade = true;
            }
            else 
            {
                return null;
            }
        }

        GameObject buildingObject = GameObject.Instantiate(metaData.prefab, tile.transform.position, tile.transform.rotation);
        Building buildingComponent = buildingObject.GetComponent<Building>();
        buildingComponent.Initialize(metaData, coordinate);
        
        if(isUpgrade)
        {
            buildingComponent.UpgradeHandoverFrom(oldBuilding);
            buildings.Remove(coordinate);
            GameObject.Destroy(oldBuilding.gameObject);
        }

        buildings.Add(coordinate, buildingComponent);        
        RecalculateResources();

        InfoDetail.GetInstance().LoadData(buildingComponent);
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
            Debug.LogError("tried to build platform");
            return null;
        }
    }

    public void DestoryBuildingAtCoordinate(Vector2Int coordinate) 
    {
        Building building = BuildingAtCoordinate(coordinate);
        if(building && building.CanDestroy())
        {
            GameObject.Destroy(building.gameObject);
            RecalculateResources();
        }
    }

    public bool CanBuild(ObjectMetaData buildingMetaData, Vector2Int coord)
    {
        GameObject tileGameObject = HexMapManager.GetInstance().TileAtCoordinate(coord);
        HexPlatform tile = tileGameObject.GetComponent<HexPlatform>();

        if(tile == null || buildingMetaData == null || !tile.inBase)
        {
            return false;
        }

        return buildingMetaData.CanBuildOn(tile.tileMetaData.id);
    }

    /// <summary>
    /// we want to update building's effects on each other
    /// </summary>
    public void RecalculateResources() 
    {
        List<Building> buildingsList = new List<Building>(buildings.Values);
        buildingsList.Sort((x, y) => x.resourceCalculationOrder.CompareTo(y.resourceCalculationOrder));
        foreach(var building in buildingsList) 
        {
            building.ResetResources();
        }

        GlobalResourceCalculation();

        foreach(var building in buildingsList) 
        {
            building.RecalculateResources();
        }
    }

    public void GlobalResourceCalculation() 
    {
        List<Building> buildingsList = new List<Building>(buildings.Values);
        buildingsList.Sort((x, y) => x.resourceCalculationOrder.CompareTo(y.resourceCalculationOrder));
            
        //GLOBAL RESOURCE DISTRIBUTION FIRST
        foreach(var consumer in buildingsList) 
        {
            if(consumer.NeedsResource(ResourceIdentifiers.OIL)) 
            {
                if(Player.GetInstance().oil != 0)
                {
                    consumer.ReceiveResource(ResourceIdentifiers.OIL, 1);
                }
            }
            
            if(consumer.NeedsResource(ResourceIdentifiers.LIGHT))
            {
                if(EnvironmentManager.GetInstance().isDay)
                {
                    consumer.ReceiveResource(ResourceIdentifiers.LIGHT, EnvironmentManager.GetInstance().daylight);
                }
            }
        }
    }

    public List<Building> NeighboursOf(Building building)
    {
        List<Building> neighbours = new List<Building>();
        List<Vector2Int> neighbourCoordinates = Hexagon.NeighbourCoordinatesOfHexagon(building.coordinate);

        foreach (Vector2Int coordinate in neighbourCoordinates)
        {
            Building neighbour = null;
            buildings.TryGetValue(coordinate, out neighbour);
            if(neighbour) 
            {
                neighbours.Add(neighbour);
            }
        }

        return neighbours;
    }

    public List<Building> NeighboursOf(Building building, int axis)
    {
        List<Building> neighbours = new List<Building>();
        List<Vector2Int> neighbourCoordinates = Hexagon.NeighbourCoordinatesOfHexagonWithAxis(building.coordinate, axis);

        foreach (Vector2Int coordinate in neighbourCoordinates)
        {
            Building neighbour = null;
            buildings.TryGetValue(coordinate, out neighbour);
            if(neighbour) 
            {
                neighbours.Add(neighbour);
            }
        }

        return neighbours;
    }

    public Building BuildingAtCoordinate(Vector2Int coord)
    {
        Building building = null;
        buildings.TryGetValue(coord, out building);
        return building;
    }

    private static BuildingsManager _singleton;

    BuildingsManager() {
        _singleton = this;
    }

    public static BuildingsManager GetInstance() 
    {
        return _singleton;
    }
}