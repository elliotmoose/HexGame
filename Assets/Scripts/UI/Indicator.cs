using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Indicator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject messageContainer;
    public Text messageText;
    
    public void Initialize(ResourceIdentifiers resourceId, string message) 
    {
        GetComponent<Image>().sprite = PrefabManager.SpriteForResourceId(resourceId);
        messageText.text = message;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        messageContainer.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        messageContainer.SetActive(false);
    }
}
