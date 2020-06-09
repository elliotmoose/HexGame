using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Controls : MonoBehaviour
{
    HexPlatform lastHovered;
    HexPlatform target = null;
    

    private bool _isManualPanning = false;
    private float panSpeed = 14;
    private int tabSize = 128;

    void Update()
    {
        UpdatePlatformSelection();
        UpdateScreenFocus();
        UpdateScreenMovement();      
    }

    void UpdatePlatformSelection()
    {        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);

        HexPlatform platform = null;
        if (hasHit)
        {
            platform = hit.transform.gameObject.GetComponent<HexPlatform>();
            if(platform == null) 
            {
                var parent = hit.transform.parent;
                
                if(parent != null) 
                {
                    platform = parent.GetComponent<HexPlatform>();
                }
            }

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && platform != null)
            {
                var coord = platform.coordinate;
                _isManualPanning = false;
                if(platform.id != Identifiers.NULL) 
                {
                    Shop.GetInstance().Open(platform);    
                }
                else 
                {
                    // Shop.GetInstance().Close();    
                }
            }

            HexPlatform lastLastHovered = null;
            if (lastHovered != platform && platform != null)
            {
                lastLastHovered = lastHovered;
                platform.SetHovered(true);
                lastHovered = platform;

                if (lastLastHovered)
                {
                    lastLastHovered.SetHovered(false);
                }
            }

        }
        else if (lastHovered)
        {
            lastHovered.SetHovered(false);
            lastHovered = null;
        }
    }

    void UpdateScreenMovement() 
    {
        float intensity = 0;
        Vector3 mouse = Input.mousePosition;
        Vector3 center = new Vector3(Screen.width/2,Screen.height/2,0);
        Vector3 mouseDir = mouse - center;  
        Vector3 dir = new Vector3(-mouseDir.y, 0, mouseDir.x);
        Vector3.Normalize(dir);
 
        if(Screen.width - tabSize < mouse.x)
            intensity -= ( (Screen.width - mouse.x) - tabSize )/ tabSize;
        if(tabSize > mouse.x)
            intensity += (tabSize - mouse.x)/ tabSize;
        if(Screen.height - tabSize < mouse.y)
            intensity -= ( ( Screen.height - mouse.y) - tabSize )/ tabSize;
        if(tabSize > mouse.y)
            intensity += (tabSize - mouse.y)/ tabSize;

        if(intensity != 0)
        {
            _isManualPanning = true;
        }
        // intensity /= 1000;
        transform.Translate(panSpeed * dir.normalized * Mathf.Min(intensity, 1) * Time.deltaTime, Space.World);

        // float scrollSpeed = 10; 
        // float threshold = 0.05f;
        
        // if(Input.mousePosition.x >= Screen.width * (1-threshold)) 
        // {
            
        //     Camera.main.transform.position += new Vector3(0,0, scrollSpeed) * Time.deltaTime;
        // }
        // if(Input.mousePosition.x <= Screen.width * threshold) 
        // {
        //     _isManualPanning = true;
        //     Camera.main.transform.position -= new Vector3(0, 0, scrollSpeed) * Time.deltaTime;
        // }
        // if(Input.mousePosition.y >= Screen.height * (1-threshold)) 
        // {
        //     _isManualPanning = true;
        //     Camera.main.transform.position -= new Vector3(scrollSpeed,0,0) * Time.deltaTime;
        // }
        // if(Input.mousePosition.y <= Screen.height * threshold) 
        // {
        //     _isManualPanning = true;
        //     Camera.main.transform.position += new Vector3(scrollSpeed,0, 0) * Time.deltaTime;
        // }
    }
    

    void UpdateScreenFocus()
    {
        if(!Shop.GetInstance().selectedPlatform || _isManualPanning) 
        {
            return;
        }
        
        // Debug.Log(Camera.main.transform.rotation.eulerAngles);
        float angleInRadians = (90-Camera.main.transform.rotation.eulerAngles.x)*Mathf.PI/180;
        float oppositeOverAdjacent = Mathf.Tan(angleInRadians);
        float horOffset = oppositeOverAdjacent * Camera.main.transform.position.y;
        Vector3 cameraOffset = new Vector3(-horOffset, 0,-1f);
        // Vector3 delta = Vector3.zero - (Camera.main.transform.position + cameraOffset);
        Vector3 delta = Shop.GetInstance().selectedPlatform.transform.position - (Camera.main.transform.position + cameraOffset);
        Vector3 deltaXZ = new Vector3(delta.normalized.x , 0 , delta.normalized.z);

        float speed = Mathf.Max(4 * delta.magnitude, 1);
        Camera.main.transform.position += deltaXZ * Time.deltaTime * speed;
    }
}
 
 
