using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class EndlessRunnerPlayer : MonoBehaviour
{
    public GroundChunkManager chunkManager;
    public ObstacleManager obstacleManager;
    [SerializeField]
    float speedMultiplier = 1.0f;
    [SerializeField]
    Renderer modelRenderer;

    int matchMaterialIndex;
    // Start is called before the first frame update
    void Start()
    {
        int num = obstacleManager.GetNumMaterials();

        matchMaterialIndex = Random.Range(0, num);
        Material mat = obstacleManager.GetMaterial(matchMaterialIndex);
        modelRenderer.material = mat;
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

    private void OnCollisionEnter(Collision collision)
    {
        var obst = collision.gameObject.GetComponent<ClickableObstacle>();
        if (obst != null)
        {
            if (obst.GetMaterialIndex() != matchMaterialIndex)
            {
                // die
            }
            else
            {
                Destroy(collision.gameObject, 0.1f);
            }
        }
    }
}
