using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollow : MonoBehaviour
{
    public Text follow;
    public Camera UICamera;
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // follow.transform.position = worldToUISpace(canvas, this.transform.position);
    }

   
}
