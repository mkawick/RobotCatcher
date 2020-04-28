using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GroundChunkManager gcm;
    public GameObject[] obstacles;
    public float currentDifficulty = 1;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(gcm != null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChunkAdded(GameObject chunk)
    {
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
            int whichObstacle = i % obstacles.Length;
            //float height = Random.RandomRange(1, 25);
            float height = 0.68f;
            float angle = Random.Range(0, 85);

            Quaternion q = obstacles[whichObstacle].transform.rotation;
            q *= Quaternion.Euler(Vector3.up * angle);
            float x = Random.Range(positionMinX, positionMaxX);
            float z = Random.Range(positionMinZ, positionMaxZ);
            Vector3 pos = new Vector3(x, height, z);
            GameObject newChunk = Instantiate(obstacles[whichObstacle], pos, q );
            newChunk.SetActive(true);
        }

    }
}
