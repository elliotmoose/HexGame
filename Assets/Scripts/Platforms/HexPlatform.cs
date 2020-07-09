using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexPlatform : MonoBehaviour
{
    public bool inBase = true;
    // public ObjectMetaData metaData;
    public TileIdentifiers id;
    public Vector2Int coordinate;

    private Material defaultMaterial;

    private Building _building;
    public Building building {
        get {
            return _building;
        }

        set {
            _building = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(!defaultMaterial)
        {
            defaultMaterial = GetComponentInChildren<Renderer>().material;
        }
    }

    public void UpdateBorder()
    {
        Mesh border = new Mesh();
        this.transform.GetChild(0).GetComponent<MeshFilter>().mesh = border;
        if(!inBase)
        {
            return;   
        }
        

        float width = HexMapManager.HEXAGON_FLAT_WIDTH;        
        
        Vector3[] vertices = new Vector3[24];
        int[] triangles = new int[36];

        for(int i=0; i< 6; i++)
        {
            Vector2Int neighbourCoord = Hexagon.NeighbourAtIndex(this.coordinate, i);
            GameObject neighbourTileGameObject = HexMapManager.GetInstance().TileAtCoordinate(neighbourCoord);
            if(neighbourTileGameObject != null && neighbourTileGameObject.GetComponent<HexPlatform>().inBase)
            {
                continue;
            }
            Vector3[] outer = Hexagon.FlatEdgeVertexPositions(width, i*60 + 60);
            // Vector3[] inner = Hexagon.FlatEdgeVertexPositions(width-0.4f, i*60 + 60);
            Vector3[] inner = Hexagon.WideFlatEdgeVertexPositions(width, width-0.4f, i*60 + 60);

            outer.CopyTo(vertices, i*4);
            inner.CopyTo(vertices, i*4 + 2);

            triangles[i*6] = i*4;
            triangles[i*6+1] = i*4+1;
            triangles[i*6+2] = i*4+3;
            triangles[i*6+3] = i*4+0;
            triangles[i*6+4] = i*4+3;
            triangles[i*6+5] = i*4+2;
        }

        border.vertices = vertices;
        border.triangles = triangles;
        border.RecalculateNormals();
    }

    public void Initialize(TileIdentifiers tileId, Vector2Int coord) {
        this.id = tileId;
        coordinate = coord;
    }

    public void SetValidation(bool isOn, bool isValid) {
        Material material = isOn ? (isValid ? Shop.GetInstance().validBuildMaterial : Shop.GetInstance().invalidBuildMaterial) : defaultMaterial;
        GetComponent<Renderer>().material = material;
    }

    public void Tick() 
    {
        if(building != null)
        {
            building.BuildingTick();
        }
    }

}
