using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject pathContainer;
    // Start is called before the first frame update
    void Start()
    {
        if (pathContainer != null)
        {
            pathContainer.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
