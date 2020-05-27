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
    
    public void LoadData(ShopItem item)
    {
        this._item = item;

        titleText.text = item.title;
        descriptionText.text = item.description;
        priceText.text = $"{item.price}";

        GameObject displayObject = GameObject.Instantiate(item.prefab, itemDisplaySlot, false);
        displayObject.transform.localPosition = Vector3.zero;
        displayObject.layer = itemDisplaySlot.gameObject.layer;
    }

    public void Purchase() 
    {
        Shop.GetInstance().Purchase(_item);
    }
}