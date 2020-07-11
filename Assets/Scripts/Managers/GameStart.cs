using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

//request space drop
public class GameStart : MonoBehaviour
{
    public BuildingMetaData spaceship;
    public BuildingMetaData tree;
    public GameObject landButton;
    public GameObject selectionIndicatorPrefab;
    private GameObject _selectionIndicator;
    private HexTile _selectedTile;

    // Start is called before the first frame update
    void Start()
    {        
        Controls.GetInstance().OnSelectPlatform += OnSelectSpaceship;        
    }

    void OnDestroy() 
    {
        Controls.GetInstance().OnSelectPlatform -= OnSelectSpaceship;
    }


    void OnSelectSpaceship(HexTile tile) 
    {
        // bool valid = BuildingsManager.GetInstance().CanBuild(spaceship, platform.coordinate);
        bool valid = true;

        if(valid)
        {
            _selectedTile = tile;            
            if(!_selectionIndicator)
            {
                _selectionIndicator = GameObject.Instantiate(selectionIndicatorPrefab, tile.transform.position, tile.transform.rotation);
            }
            

            _selectionIndicator.transform.position = tile.transform.position + new Vector3(0, 0.01f, 0);
            _selectionIndicator.GetComponent<Renderer>().material = tile.GetComponent<Renderer>().material;
            _selectionIndicator.GetComponent<Renderer>().material.SetFloat("_OutlineWidth", 1.2f);
        }
        else 
        {
            UIManager.Message("Cannot build here");
        }
    }

    // Update is called once per frame
    void Update()
    {
               
    }

    public void LandPressed() 
    {
        if(!_selectedTile)
        {
            return;
        }
        GameObject buildingGo = BuildingsManager.GetInstance().Build(spaceship, _selectedTile.coordinate);
        UIManager.GetInstance().GetComponent<Animator>().Play("HUDEnter");
        if(_selectionIndicator)
        {
            GameObject.Destroy(_selectionIndicator);
        }
        GameObject.Destroy(landButton);   
        Destroy(this);   
    }

}
