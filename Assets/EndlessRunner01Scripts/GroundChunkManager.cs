using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundChunkManager : MonoBehaviour
{
    public GameObject[] workingChunks;
    List<GameObject> chunkList;
    public NavMeshSurface navMeshSurface;
    float playerZ = 0;
    float distanceBeforeCreatingNewChunk = 45;
    float chunkLength = 15;
    float lastDistanceChunk;
    Vector3 lastChunkPositionPlaced;
    // Start is called before the first frame update
    void Start()
    {
        chunkList = new List<GameObject>();
        
        SetupScene();
        HideAllWorkingChunks();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HideAllWorkingChunks()
    {
        foreach (var i in workingChunks)
            i.SetActive(false);
    }
    void SetupScene()
    {
        Vector3 position = new Vector3(0, 0, 0);
        for (int i = 0; i < 3; i++)
        {
            GameObject floor = AddChunkToWorld(position);
            chunkList.Add(floor);
            float length = floor.transform.lossyScale.z;
            if (i == 0)
                position.z += length;// centered at origin
            else
                position.z += length;
            floor.SetActive(true);
        }
        distanceBeforeCreatingNewChunk = position.z;
        lastDistanceChunk = position.z;
        lastChunkPositionPlaced = position;

        chunkLength = workingChunks[0].transform.lossyScale.z;// todo, generalize this
        Debug.Log("chunk length: " + chunkLength);

        RebuildNavMesh();
    }

    void RebuildNavMesh()
    {
        if (navMeshSurface == null)
            return;

        var settings = navMeshSurface.GetBuildSettings();

        settings.agentRadius = 0.5f;
        settings.agentHeight = 2.0f;
        settings.agentSlope = 23;
        settings.agentClimb = 0.4f;
        settings.minRegionArea = 2.0f;

        navMeshSurface.BuildNavMesh();
    }

    GameObject AddChunkToWorld(Vector3 position)
    {
        float which = Random.Range(0, workingChunks.Length);
        GameObject newChunk = Instantiate(workingChunks[(int)which], this.transform );
        newChunk.transform.position = position;
        newChunk.SetActive(true);
        return newChunk;
    }

    void DeleteOldChunk()
    {
        GameObject go = chunkList[0];
        chunkList.RemoveAt(0);
        Destroy(go);
        //chunkList[chunkList.Count - 1]
    }

    public void UpdateWorldPosition(Vector3 playerPosition)
    {
        if(playerZ < playerPosition.z)
        {
            playerZ = playerPosition.z;
        }

        if (lastChunkPositionPlaced.z + chunkLength  < playerZ + distanceBeforeCreatingNewChunk)
        {
            Vector3 v = lastChunkPositionPlaced;
            //v.z += ;
            GameObject floor = AddChunkToWorld(v);
            chunkList.Add(floor);

            MoveTheLastChunkTrackingForward();
            DeleteOldChunk();
            RebuildNavMesh();
        }
    }

    void MoveTheLastChunkTrackingForward()
    {
        lastChunkPositionPlaced = chunkList[chunkList.Count - 1].transform.position;
        lastChunkPositionPlaced.z += chunkLength;
    }
     
}
