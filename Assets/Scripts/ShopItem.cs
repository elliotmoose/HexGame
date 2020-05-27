using UnityEngine;

public class ShopItem 
{
    public string title;
    public string description;
    public float price;
    public GameObject prefab;

    public ShopItem(string title, string description, float price, GameObject prefab)
    {
        this.title = title;
        this.description = description;
        this.price = price;
        this.prefab = prefab;
    }
}
