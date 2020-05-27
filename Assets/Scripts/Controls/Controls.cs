using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Controls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    HexPlatform lastHovered;
    AttachmentType selectedAttachmentType = AttachmentType.HARVESTER;

    void Update()
    {
        UpdatePlatformSelection();
        UpdateScreenMovement();      
    }

    void UpdatePlatformSelection()
    {        
        if(EventSystem.current.IsPointerOverGameObject()) 
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);

        HexPlatform platform = null;
        if (hasHit)
        {
            platform = hit.transform.gameObject.GetComponent<HexPlatform>();

            if (Input.GetMouseButtonDown(0) && platform)
            {
                var coord = platform.coordinate;

                if(platform.platformType != PlatformType.NONE) 
                {
                    Shop.GetInstance().Open(platform);    
                }
                else 
                {
                    Shop.GetInstance().Close();    
                }
                
                // if (platform.tileType == PlatformType.PLACEHOLDER)
                // {
                //     Shop.GetInstance().BuildPlatform(PlatformType.SOIL, platform.coordinate);
                // }
                // else if (platform.tileType == PlatformType.MINERAL)
                // {
                //     Shop.GetInstance().BuildAttachment(AttachmentType.HARVESTER, platform.coordinate);
                // }
                // else if (platform.tileType == PlatformType.SOIL)
                // {
                //     Shop.GetInstance().BuildBuilding(BuildingType.TREE, platform.coordinate);
                // }
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

    float panSpeed = 14;
    int tabSize = 128;

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
        //     Camera.main.transform.position -= new Vector3(0, 0, scrollSpeed) * Time.deltaTime;
        // }
        // if(Input.mousePosition.y >= Screen.height * (1-threshold)) 
        // {
        //     Camera.main.transform.position -= new Vector3(scrollSpeed,0,0) * Time.deltaTime;
        // }
        // if(Input.mousePosition.y <= Screen.height * threshold) 
        // {
        //     Camera.main.transform.position += new Vector3(scrollSpeed,0, 0) * Time.deltaTime;
        // }
    }
}
 
 
