using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTextureGenerator : MonoBehaviour
{
    public float textureHeight = 0.5f;
    public float scale = 20f;

    //Position where the waves originate from
    public Vector3 waveOriginPosition = new Vector3(0.0f, 0.0f, 0.0f);

    MeshFilter meshFilter;
    Mesh mesh;
    Vector3[] vertices;

    void Start() 
    {
        Build();
    }
    public void Build()
    {
        meshFilter = GetComponent<MeshFilter>();
        CreateMeshLowPoly(meshFilter);      
        GenerateWaves();
    }

    void Update()
    {
    }

    /// <summary>
    /// Rearranges the mesh vertices to create a 'low poly' effect
    /// </summary>
    /// <param name="mf">Mesh filter of gamobject</param>
    /// <returns></returns>
    MeshFilter CreateMeshLowPoly(MeshFilter mf)
    {
        mesh = mf.sharedMesh;

        //Get the original vertices of the gameobject's mesh
        Vector3[] originalVertices = mesh.vertices;

        //Get the list of triangle indices of the gameobject's mesh
        int[] triangles = mesh.triangles;

        //Create a vector array for new vertices 
        Vector3[] vertices = new Vector3[triangles.Length];

        //Assign vertices to create triangles out of the mesh
        for (int i = 0; i < triangles.Length; i++)
        {
            vertices[i] = originalVertices[triangles[i]];
            triangles[i] = i;
        }

        //Update the gameobject's mesh with new vertices
        mesh.vertices = vertices;
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        this.vertices = mesh.vertices;

        return mf;
    }

    void GenerateWaves()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 v = vertices[i];

            //Initially set the wave height to 0
            v.y = 0.0f;
            //Get the distance between wave origin position and the current vertex
            // float distance = Vector3.Distance(v, waveOriginPosition);
            // distance = (distance % waveLength) / waveLength;

            //Oscilate the wave height via sine to create a wave effect
            // v.y = waveHeight * Mathf.Sin(Time.time * Mathf.PI * 2.0f * waveFrequency
            // + (Mathf.PI * 2.0f * distance));

            int mapSize = 10;
            float size = mapSize*10;
            float xCoord = (float)(v.x+size/2)/size * scale;
            float yCoord = ((float)(v.z+size/2))/size * scale;
            v.y = Mathf.PerlinNoise(xCoord, yCoord) * textureHeight;
            vertices[i] = v;
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.MarkDynamic();
        meshFilter.mesh = mesh;

    }
}
