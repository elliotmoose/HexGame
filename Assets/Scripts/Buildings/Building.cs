using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Identifiers id = Identifiers.NULL;
    public Vector2Int coordinate = Vector2Int.zero;

    protected List<Building> neighbourBuildings {
        get {
            HexPlatform buildingPlatform = PlatformManager.GetInstance().PlatformAtCoordinate(this.coordinate);
            List<HexPlatform> platforms = PlatformManager.GetInstance().NeighboursOfPlatform(buildingPlatform);
            List<Building> neighbours = new List<Building>();

            foreach (HexPlatform platformNeighbour in platforms)
            {
                if(platformNeighbour.building)
                {
                    neighbours.Add(platformNeighbour.building);
                }
            }

            return neighbours;
        }
    }

    public void Initialize(Identifiers id, Vector2Int coord) {
        this.id = id;
        coordinate = coord;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
