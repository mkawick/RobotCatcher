using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class rebaker : MonoBehaviour
{
    //public  GameObject WalkableTerrainHolder; // assumes root is ground
    public NavMeshSurface surface;
    public Transform plane;

    void Start()
    {
      /*  surface = WalkableTerrainHolder.GetComponent<NavMeshSurface>();
        if (surface == null)
            surface = WalkableTerrainHolder.AddComponent<NavMeshSurface>();*/

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) == true)
        {
            //Rebake();
            RegenerateNavMesh();
        }
        if (Input.GetKey(KeyCode.UpArrow) == true)
        {
            MakeSceneLarger();
        }
    }

    private void MakeSceneLarger()
    {
        plane.localScale += new Vector3(.1f, .1f, .1f);
    }

    private void Rebake()
    {
        // Collect all children
        //List<GameObject> childList = new List<GameObject>();
        //CollectChildren(WalkableTerrainHolder, childList);
        //WalkableTerrainHolder.GetComponent<NavMeshSurface>().collectObjects = CollectObjects.Children;
        //RegenerateNavMesh();
    }

   /* void CollectChildren(GameObject parent, List<GameObject> childList)
    {
        var listOfCrap = parent.GetComponentsInChildren<Transform>(); 
        foreach(var obj in listOfCrap)
        {
            CollectChildren(obj.gameObject, childList);
            
        }
        var NavObstacle = parent.GetComponent<NavMeshObstacle>();
        if(NavObstacle != null)
        {
            childList.Add(parent);
        }
    }*/

    void RegenerateNavMesh()
    {
        //var settings = NavMesh.CreateSettings();
        

        
        //var surface = WalkableTerrainHolder.GetComponent<NavMeshSurface>();
        //surface.useGeometry
        var settings = surface.GetBuildSettings();
        var navMeshData = surface.navMeshData;

        settings.agentRadius = 0.5f;
        settings.agentHeight = 2.0f;
        settings.agentSlope = 30;
        settings.agentClimb = 0.4f;

        settings.minRegionArea = 2.0f;
        //settings.mesh
        //navMeshData.

        Debug.Log("BAking");
        surface.BuildNavMesh();
    }
}
