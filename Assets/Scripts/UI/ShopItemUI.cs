using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{

    public Text titleText;
    public Text descriptionText;
    public Text priceText;
    public Transform itemDisplaySlot;

    private ShopItem _item;

    private GameObject _displayObject;
    
    public void LoadData(ShopItem item)
    {
        this._item = item;

        titleText.text = item.title;
        descriptionText.text = item.description;
        priceText.text = $"{item.price}";

        _displayObject = GameObject.Instantiate(item.prefab, itemDisplaySlot, false);
        _displayObject.transform.localPosition = Vector3.zero;
        _displayObject.layer = itemDisplaySlot.gameObject.layer;
        foreach (Transform child in _displayObject.transform)
        {
            child.gameObject.layer = itemDisplaySlot.gameObject.layer;
        }
    }

    float rotateSpeed = 75;
    
    void Update()
    {
        if(_displayObject)
        {
            _displayObject.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        }
    }   

    public void Purchase() 
    {
        Shop.GetInstance().Purchase(_item);
    }
}