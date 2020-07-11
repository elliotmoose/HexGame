using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hexagon 
{
    public static float HEXAGON_FLAT_WIDTH = 2;
    public static Vector3[] FlatEdgeVertexPositions(float hexFlatSideToSideWidth, float angle)
    {
        Vector3[] vertexPoints = new Vector3[2];

        float radius = hexFlatSideToSideWidth/2;
        float hexFlatSideWidth = hexFlatSideToSideWidth / (2 * Mathf.Sin(Mathf.PI / 3));
        vertexPoints[0] = RotatePointAroundPivot(new Vector3(hexFlatSideWidth/2,0.001f, -radius), Vector3.zero, angle);
        vertexPoints[1] = RotatePointAroundPivot(new Vector3(-hexFlatSideWidth/2,0.001f, -radius), Vector3.zero, angle);
        return vertexPoints;
    }
    
    public static Vector3[] WideFlatEdgeVertexPositions(float hexFlatSideToSideWidth, float targetFlatSideToSideWidth, float angle)
    {
        Vector3[] vertexPoints = new Vector3[2];

        float radius = targetFlatSideToSideWidth/2;
        float fraction = targetFlatSideToSideWidth/hexFlatSideToSideWidth;
        float hexFlatSideWidth = hexFlatSideToSideWidth / (2 * Mathf.Sin(Mathf.PI / 3));

        float edgeWidth = Mathf.Lerp(hexFlatSideWidth, hexFlatSideWidth*2, 1-fraction);
        vertexPoints[0] = RotatePointAroundPivot(new Vector3(edgeWidth/2,0.001f, -radius), Vector3.zero, angle);
        vertexPoints[1] = RotatePointAroundPivot(new Vector3(-edgeWidth/2,0.001f, -radius), Vector3.zero, angle);
        return vertexPoints;
    }

    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, float angle) {
        return Quaternion.Euler(0, angle, 0) * (point - pivot) + pivot;
    }

    public static Vector3 PositionForCoordinate(Vector2Int coord)
    {
        var x = coord.x;
        var y = coord.y;
        float hexEdge = HEXAGON_FLAT_WIDTH / (2 * Mathf.Sin(Mathf.PI / 3));
        float verticalOffset = hexEdge * 3 / 2;
        bool isOddRow = (x % 2 != 0);
        bool isOddColumn = (y % 2 != 0);
        float horizontalOffsetOddRow = isOddRow ? -HEXAGON_FLAT_WIDTH / 2 : 0;

        return new Vector3(x * verticalOffset, 0, y * 2 - horizontalOffsetOddRow);
    }

    public static List<Vector2Int> NeighbourCoordinatesOfHexagon(Vector2Int coord) 
    {        
        List<Vector2Int> neighbours = new List<Vector2Int>();

        var isOdd = ((coord.x % 2) == 0);
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
            neighbours.Add(new Vector2Int(coord.x + offset[0], coord.y + offset[1]));
        }

        return neighbours;
    }

    public static List<Vector2Int> NeighbourCoordinatesOfHexagonWithAxis(Vector2Int coord, int axis) 
    {        
        List<Vector2Int> neighbours = new List<Vector2Int>();

        var isOdd = ((coord.x % 2) == 0);
        int[][] neighbourOffsets = new int[2][];

        switch (axis)
        {
            case 0:
                neighbourOffsets = new int[][]{
                    new int[] { 0, 1 }, 
                    new int[] { 0, -1 }
                };
                break;
            case 1:
                if(isOdd)
                {
                    neighbourOffsets = new int[][]{
                        new int[]{-1, 0},
                        new int[]{1,-1},
                    };
                }
                else 
                {
                    neighbourOffsets = new int[][]{
                        new int[]{1, 0},
                        new int[]{-1, 1},
                    };                           
                }
                break;
            case 2:
                if(isOdd)
                {
                    neighbourOffsets = new int[][]{
                        new int[]{1, 0},
                        new int[]{-1, -1},
                    };
                }
                else 
                {
                    neighbourOffsets = new int[][]{
                        new int[]{-1, 0},
                        new int[]{1,1},
                    };    
                }
                break;
            default:
                Debug.Log($"invalid axis: {axis}");
                break;
        }

        foreach (int[] offset in neighbourOffsets)
        {
            neighbours.Add(new Vector2Int(coord.x + offset[0], coord.y + offset[1]));
        }

        return neighbours;
    }

    public static Vector2Int NeighbourAtIndex(Vector2Int coord, int index) 
    {
        var isOdd = ((coord.x % 2) == 0);
        if(isOdd)
        {
            int[] offset = new int [][]{
                new int[]{-1, -1},
                new int[]{-1, 0},
                new int[]{0, 1}, 
                new int[]{1, 0},
                new int[]{1, -1},
                new int[]{0, -1},
            }[index];

            return new Vector2Int(coord.x + offset[0], coord.y + offset[1]);
        }
        else 
        {
            int[] offset = new int [][]{
                new int[]{-1, 0},
                new int[]{-1, 1},
                new int[]{0, 1}, 
                new int[]{1, 1},
                new int[]{1, 0},
                new int[]{0, -1},
            }[index];

            return new Vector2Int(coord.x + offset[0], coord.y + offset[1]);

        }
    }
}