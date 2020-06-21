using UnityEngine;
using UnityEngine.UI;

public class UIHelper 
{
    public static Vector3 WorldToUISpace(Vector3 worldPos, Canvas canvas, Camera camera)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, camera, out movePos);
        //Convert the local point to world point
        return canvas.transform.TransformPoint(movePos);
    }
}