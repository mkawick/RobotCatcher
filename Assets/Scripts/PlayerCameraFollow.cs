using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    Vector3 cameraOffset;
    [SerializeField]
     Camera mainCamera;
    [SerializeField]
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        if (mainCamera == null || player == null)
            return;

        cameraOffset = player.position - mainCamera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraOffset == null)
            return;
        mainCamera.transform.position = player.position - cameraOffset;
    }
}
