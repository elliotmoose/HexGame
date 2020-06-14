using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupText : MonoBehaviour
{
    private GameObject _target;

    public void Initialize(string text, GameObject target) 
    {
        _target = target;
        foreach(Text textComponent in GetComponentsInChildren<Text>())
        {
            textComponent.text = text;
        }
        StartCoroutine(FloatUpAndDestroyAfter(0.7f));
    }

    void Update() 
    {
    }

    IEnumerator FloatUpAndDestroyAfter(float lifetime) 
    {
        float time = 0;
        Vector3 floatOffset = Vector3.zero;        
        while(time < lifetime)
        {            
            time += Time.deltaTime;
            floatOffset = new Vector3(0,0.75f,0) * Mathf.Lerp(0, 1, time/lifetime);
        
            foreach(Text textComponent in GetComponentsInChildren<Text>())
            {
                Color color = textComponent.color;
                color.a  = Mathf.Lerp(1,0,time/lifetime);
                textComponent.color = color;
            }
            this.transform.position = UIManager.WorldToUISpace(_target.transform.position) + floatOffset + new Vector3(0,1,0);        
            yield return null;            
        }
        
        GameObject.Destroy(this.gameObject);
        yield return null;
    }
}
