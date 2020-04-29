using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObstacle : MonoBehaviour
{
    int matchMaterialIndex = 0;
    public ObstacleManager obstacleManager;
    Renderer modelRenderer;

    // Start is called before the first frame update
    void Start()
    {
        modelRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void RandomizeIndex()
    {
        int num = obstacleManager.GetNumMaterials();

        matchMaterialIndex = Random.Range(0, num);
        SetupMaterial();
    }
    private void OnMouseDown()
    {
        int num = obstacleManager.GetNumMaterials();
        matchMaterialIndex++;
        if (matchMaterialIndex >= num)
        {
            matchMaterialIndex = 0;
        }
        SetupMaterial();
    }

    void SetupMaterial()
    {
        Material mat = obstacleManager.GetMaterial(matchMaterialIndex);
        GetComponent<Renderer>().material = mat;
    }
}
