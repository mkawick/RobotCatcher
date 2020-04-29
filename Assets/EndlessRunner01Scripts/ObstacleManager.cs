﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GroundChunkManager gcm;
    public GameObject[] obstaclePrefabs;
    public float currentDifficulty = 1;
    public Material[] matchMaterials;
    public GameObject obstacleContainer;

    public bool shouldGenerateObstacles = true;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(gcm != null);
        HidePrefabs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HidePrefabs()
    {
        foreach(var p in obstaclePrefabs)
        {
            p.SetActive(false);
        }
    }

    public void ChunkAdded(GameObject chunk)
    {
        if (shouldGenerateObstacles == false)
            return;

        Vector3 scale = chunk.transform.localScale;
        NewChunkWasGenerated(chunk.transform.position, scale.x/2, scale.z/2);
    }

    void NewChunkWasGenerated(Vector3 center, float boundsX, float boundsZ)
    {
        float rangeMin = currentDifficulty * 3;
        float rangeMax = currentDifficulty * 5;

        if (rangeMax > 100)
            rangeMax = 100;
        float numObstaclesToGenerate = Random.Range(rangeMin, rangeMax);

        float margin = 0.5f;
        float positionMinX = center.x - boundsX + margin;
        float positionMaxX = center.x + boundsX - margin;
        float positionMinZ = center.z - boundsZ;
        float positionMaxZ = center.z + boundsZ;
        for (int i=0; i< numObstaclesToGenerate; i++)
        {
            int whichObstacle = i % obstaclePrefabs.Length;
            //float height = Random.RandomRange(1, 25);
            float height = 0.68f;
            float angle = Random.Range(0, 85);

            Quaternion q = obstaclePrefabs[whichObstacle].transform.rotation;
            q *= Quaternion.Euler(Vector3.up * angle);
            float x = Random.Range(positionMinX, positionMaxX);
            float z = Random.Range(positionMinZ, positionMaxZ);
            Vector3 pos = new Vector3(x, height, z);
            CreateObstacle(whichObstacle, pos, q);
        }
    }

    void CreateObstacle(int which, Vector3 pos, Quaternion q)
    {
        GameObject newChunk = Instantiate(obstaclePrefabs[which], pos, q);        
        ClickableObstacle obst = newChunk.GetComponent<ClickableObstacle>();
        if (obst)
        {
            obst.obstacleManager = this;
        }
        newChunk.SetActive(true);
        obst.transform.parent = obstacleContainer.transform;
        obst.RandomizeIndex();
    }

    internal int GetNumMaterials() { return matchMaterials.Length; }
    internal Material GetMaterial(int which)
    {
        if (which >= matchMaterials.Length)
            return null;

        return matchMaterials[which];
    }
}
