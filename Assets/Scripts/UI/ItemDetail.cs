using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetail : MonoBehaviour
{
    public GameObject shopItemDescriptionContainer;
    public GameObject metricsDetailsContainer;

    public Transform itemDisplaySlot;
    public Text titleText;
    public Text descriptionText;
    public Text statsText;
    public Text costText;

    private GameObject _displayObject;


    public void LoadData(ObjectMetaData data)
    {
        if(_displayObject)
        {
            GameObject.Destroy(_displayObject);
        }

        _displayObject = GameObject.Instantiate(data.displayPrefab, itemDisplaySlot, false);
        _displayObject.transform.localPosition = Vector3.zero;
        _displayObject.layer = itemDisplaySlot.gameObject.layer;
        foreach (Transform child in _displayObject.transform)
        {
            child.gameObject.layer = itemDisplaySlot.gameObject.layer;
        }

        titleText.text = data.title;
        costText.text = $"{data.price}";
        descriptionText.text = data.description;

        string statsString = "";

        foreach(var param in data.parameters)
        {
            statsString += $"• {param.key}: {param.value}\n";
        }

        statsText.text = statsString;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_displayObject)
        {
            _displayObject.transform.Rotate(0, 75 * Time.deltaTime, 0);
        }
    }
}
