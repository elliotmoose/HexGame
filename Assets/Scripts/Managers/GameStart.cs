using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

//request space drop
public class GameStart : MonoBehaviour
{
    public BuildingMetaData spaceship;
    public BuildingMetaData tree;

    // Start is called before the first frame update
    void Start()
    {        
        Controls.GetInstance().OnSelectPlatform += OnSelectSpaceship;        
    }

    void OnDestroy() 
    {
        Controls.GetInstance().OnSelectPlatform -= OnSelectSpaceship;
    }

    void OnSelectSpaceship(HexPlatform platform) 
    {
        // bool valid = BuildingsManager.GetInstance().CanBuild(spaceship, platform.coordinate);
        bool valid = true;

        if(valid)
        {
            GameObject buildingGo = BuildingsManager.GetInstance().Build(spaceship, platform.coordinate);
            TreeBuilding treeBuilding = buildingGo.transform.GetChild(1).gameObject.GetComponent<TreeBuilding>();
            treeBuilding.GrowUp();
            treeBuilding.Initialize(tree, platform.coordinate);
            UIManager.GetInstance().GetComponent<Animator>().Play("HUDEnter");
            Destroy(this);   
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
}
