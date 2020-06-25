using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{

    // public Text titleText;
    // public Text descriptionText;
    // public Text priceText;
    public Transform itemDisplaySlot;
    // public Button button;

    private ObjectMetaData _item;

    private GameObject _displayObject;
    private GameObject _dragDropObject;
    
    public void LoadData(ObjectMetaData item)
    {
        this._item = item;

        // titleText.text = item.title;
        // descriptionText.text = item.description;
        // priceText.text = $"{item.price}";

        _displayObject = GameObject.Instantiate(item.displayPrefab, itemDisplaySlot, false);
        _displayObject.transform.localPosition = Vector3.zero;
        _displayObject.layer = itemDisplaySlot.gameObject.layer;
        foreach (Transform child in _displayObject.transform)
        {
            child.gameObject.layer = itemDisplaySlot.gameObject.layer;
        }
    }

    
    void Update()
    {
        if(_displayObject)
        {
            _displayObject.transform.Rotate(0, UIManager.ITEM_DISPLAY_ROTATION_SPEED * Time.deltaTime, 0);
        }
    }   

    public void StartDrag() 
    {
        Controls.GetInstance().BeginDragAndDrop(_item);        
    }

    public void ShowDetails()
    {
        UIManager.GetInstance().HoverDetailsEnter(this._item, this.transform.position);
    }

    public void HideDetails()
    {
        UIManager.GetInstance().HoverDetailsExit(this._item);
    }
}