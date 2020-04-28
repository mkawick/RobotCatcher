using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class EndlessRunnerPlayer : MonoBehaviour
{
    public GroundChunkManager chunkManager;
    [SerializeField]
    float speedMultiplier = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        chunkManager.UpdateWorldPosition(transform.position);
        MoveForward();
    }

    void MoveForward()
    {
        Vector3 moveDir = Vector3.forward;

        var control = GetComponent<ThirdPersonCharacter>();
        control.MoveSpeedMultiplier = speedMultiplier;
        control.AnimSpeedMultiplier = speedMultiplier;
        control.Move(moveDir, false, false);

    }
}
