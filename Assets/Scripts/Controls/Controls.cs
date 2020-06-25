using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Controls : MonoBehaviour
{
    HexPlatform lastHovered;
    
    private ObjectMetaData _selectedShopItem;
    private GameObject _dragDropObject;

    private bool _isManualPanning = false;
    private float panSpeed = 14;
    private int tabSize = 45;

    void Update()
    {
        UpdatePlatformSelection();
        UpdateScreenFocus();
        // UpdateScreenMovement();      
        UpdateDragAndDrop();
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
                if(platform.metaData.id != Identifiers.NULL) 
                {   
                    if(platform.building)
                    {
                        InfoDetail.GetInstance().LoadData(platform.building);
                    }
                    // Shop.GetInstance().Open(platform);    
                }
                else 
                {
                    // Shop.GetInstance().Close();    
                }
            }

            // HexPlatform lastLastHovered = null;
            // if (lastHovered != platform && platform != null)
            // {
            //     lastLastHovered = lastHovered;
            //     // platform.SetHovered(true);
            //     lastHovered = platform;

            //     if (lastLastHovered)
            //     {
            //         // lastLastHovered.SetHovered(false);
            //     }
            // }

        // }
        // else if (lastHovered)
        // {
        //     lastHovered.SetHovered(false);
        //     lastHovered = null;
        }
    }

    void UpdateScreenMovement() 
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _isManualPanning = false;
        }

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
        if(!PlatformManager.GetInstance().selectedPlatform || _isManualPanning) 
        {
            return;
        }
        
        // Debug.Log(Camera.main.transform.rotation.eulerAngles);
        float angleInRadians = (90-Camera.main.transform.rotation.eulerAngles.x)*Mathf.PI/180;
        float oppositeOverAdjacent = Mathf.Tan(angleInRadians);
        float horOffset = oppositeOverAdjacent * Camera.main.transform.position.y;
        Vector3 cameraOffset = new Vector3(-horOffset, 0,-1f);
        // Vector3 delta = Vector3.zero - (Camera.main.transform.position + cameraOffset);
        Vector3 delta = PlatformManager.GetInstance().selectedPlatform.transform.position - (Camera.main.transform.position + cameraOffset);
        Vector3 deltaXZ = new Vector3(delta.normalized.x , 0 , delta.normalized.z);

        float speed = Mathf.Max(4 * delta.magnitude, 1);
        Camera.main.transform.position += deltaXZ * Time.deltaTime * speed;
    }

    void UpdateDragAndDrop()
    {        
        //1. if there is something to drag and drop, update its position
        //1b: if theres no hit, just position the object close to camera
        //2. if it is hovering a platform, validate if that platform is a plausible place to build this item
        //3. if let go
        //3a: reset drag and drop
        //3b: purchase
        bool isDragAndDropping = (_dragDropObject != null && _selectedShopItem != null);

        if(isDragAndDropping)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hasHit = Physics.Raycast(ray, out hit);

            HexPlatform platform = null;

            #region update drag drop position
            if (hasHit)
            {
                platform = hit.transform.gameObject.GetComponent<HexPlatform>();
                
                float distanceTowardCamera = 1;
                Vector3 offsetTowardCamera = (Camera.main.transform.position - hit.point).normalized * distanceTowardCamera;
                Vector3 targetPositionInMainCamera = hit.point + offsetTowardCamera;
                // Vector3 intermediatePosition = Camera.main.WorldToViewportPoint(targetPositionInMainCamera);
                // Vector3 targetPositionInUICamera = UIManager.GetInstance().UICamera.ViewportToWorldPoint(intermediatePosition);
                _dragDropObject.transform.position = targetPositionInMainCamera;
                _dragDropObject.transform.Rotate(0, UIManager.ITEM_DISPLAY_ROTATION_SPEED * Time.deltaTime, 0);            
            }
            else 
            {
                _dragDropObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 8));
            }

            #endregion

            #region validation
            bool canBuildHere = (platform != null && PlatformManager.GetInstance().CanBuild(_selectedShopItem, platform.coordinate));

            //validation 1: must update currently hovered platform indicator
            //validation 2: must update previous hovered platform indicator, if any
            if(platform)
            {
                platform.SetValidation(true, canBuildHere);
            }

            if(lastHovered && lastHovered != platform)
            {
                lastHovered.SetValidation(false, false);
            }


            #endregion

            #region drop event
            bool triggerDropAttempt = Input.GetMouseButtonUp(0);
            if(triggerDropAttempt)
            {
                if(canBuildHere)
                {
                    Shop.GetInstance().Purchase(_selectedShopItem, platform.coordinate);
                }
                else 
                {
                    Debug.Log("Can't build here");
                }
                
                EndDragAndDrop();
            }
            #endregion

            lastHovered = platform;
        }


    }
    
    public void BeginDragAndDrop(ObjectMetaData shopItem) 
    {
        if(!_dragDropObject)
        {
            _selectedShopItem = shopItem;
            _dragDropObject = GameObject.Instantiate(shopItem.displayPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            // LayerMask layer = LayerMask.NameToLayer("UI");
            _dragDropObject.layer = 0;
            foreach (Transform child in _dragDropObject.GetComponentsInChildren<Transform>(true)) {
                child.gameObject.layer = 0;
            }

            Shop.GetInstance().shopItemsDisplay.SetActive(false);
            //close details
            UIManager.GetInstance().HoverDetailsExit(UIManager.GetInstance().currentShopItemData);
        }
    }

    private void EndDragAndDrop() 
    {
        _selectedShopItem = null;
        GameObject.Destroy(_dragDropObject);

        if(Shop.GetInstance().isOpen)
        {
            //close shop
            Shop.GetInstance().shopItemsDisplay.SetActive(true);
        }
    }
   
    private static Controls _singleton;

    Controls() {
        _singleton = this;
    }

    public static Controls GetInstance() 
    {
        return _singleton;
    }

}
 
 
