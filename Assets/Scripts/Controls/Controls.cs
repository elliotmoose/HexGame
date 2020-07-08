using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public delegate void TileEvent(HexPlatform platform);

public class Controls : MonoBehaviour
{
    HexPlatform lastHovered;
    
    public TileEvent OnSelectPlatform;
        

    private ObjectMetaData _selectedShopItem;
    private GameObject _dragDropObject;

    private bool _isManualPanning = false;
    private float panSpeed = 14;
    private int tabSize = 45;

    float maxHeight = 14;
    float minHeight = 4;
    // float maxHeight = 11;
    // float minHeight = 4.5f;
    float maxAngle = 71;
    float minAngle = 38.6f;
    float zoomProgress = 1;

    void Update()
    {
        UpdatePlatformSelection();
        UpdateScreenFocus();
        UpdatePanScreen();
        // UpdateScreenMovement();      
        UpdateDragAndDrop();        
    }

    private Vector3 touchStart;
    public float groundZ = 0;


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
                _isManualPanning = false;             
                if(OnSelectPlatform != null)
                {
                    OnSelectPlatform(platform);
                }                                
            }
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
    
    Vector3 mousePos = Vector3.zero;
    Vector3 focalPoint = Vector3.zero;


    private Vector3 GetWorldPosition(float z){
        Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, new Vector3(0,0,z));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }

    void UpdatePanScreen() 
    {   
        if(_dragDropObject)
        {
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            touchStart = GetWorldPosition(groundZ);
        }

        if(Input.GetMouseButton(0))
        {
            _isManualPanning = true;
            Vector3 direction = (touchStart - GetWorldPosition(groundZ));
            Vector3 delta = new Vector3(direction.x, 0, direction.z);
            focalPoint += delta;            
        }
        else 
        {
            _isManualPanning = false;
        }
    }

    void UpdateScreenFocus()
    {
        // if(PlatformManager.GetInstance().selectedPlatform)
        // {
        //     focalPoint = PlatformManager.GetInstance().selectedPlatform.transform.position;
        // }

        zoomProgress = Mathf.Clamp(zoomProgress + Input.mouseScrollDelta.y/100, 0, 1);

        float avgTileHeight = (HexMapManager.GetInstance().HEIGHT_CAP+HexMapManager.GetInstance().HEIGHT_FLOOR)/2;
        float height = Mathf.Lerp(minHeight, maxHeight, zoomProgress);
        float angle = Mathf.Lerp(minAngle, maxAngle, zoomProgress);
        float angleInRadians = (90-angle)*Mathf.PI/180;
        float deltaAngle = angle - Camera.main.transform.rotation.eulerAngles.x;
        float oppositeOverAdjacent = Mathf.Tan(angleInRadians);
        float horOffset = oppositeOverAdjacent * height;
        Vector3 cameraOffset = new Vector3(-horOffset, 0,0);
        Vector3 targetPosition = focalPoint + new Vector3(horOffset,height + avgTileHeight,0);
        Vector3 delta = targetPosition - Camera.main.transform.position;
        
        float speed = 8;
        if(_isManualPanning)
        {
            Camera.main.transform.position += delta;            
            Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x + deltaAngle, Camera.main.transform.rotation.eulerAngles.y, Camera.main.transform.rotation.eulerAngles.z);        
        }
        else 
        {
            Camera.main.transform.position += delta * Time.deltaTime * speed;
            Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x + deltaAngle * Time.deltaTime * speed, Camera.main.transform.rotation.eulerAngles.y, Camera.main.transform.rotation.eulerAngles.z);        
        }
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
            bool canBuildHere = (platform != null && BuildingsManager.GetInstance().CanBuild(_selectedShopItem, platform.coordinate));

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

        if(lastHovered)
        {
            lastHovered.SetValidation(false, false);
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
 
 
