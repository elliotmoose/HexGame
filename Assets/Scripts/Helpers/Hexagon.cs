using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hexagon 
{
    public static Vector3 PositionOfHexagonAtCoordinate(Vector2Int coord, float hexFlatSideToSideWidth)
    {
        var x = coord.x;
        var y = coord.y;
        float hexEdge = hexFlatSideToSideWidth / (2 * Mathf.Sin(Mathf.PI / 3));
        float verticalOffset = hexEdge * 3 / 2;
        bool isOddRow = (x % 2 != 0);
        bool isOddColumn = (y % 2 != 0);
        float horizontalOffsetOddRow = isOddRow ? -hexFlatSideToSideWidth / 2 : 0;

        return new Vector3(x * verticalOffset, 0, y * 2 - horizontalOffsetOddRow);
    }
}