using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTagSwarmBot : MonoBehaviour
{
    [SerializeField]
    GameManagerBotSwarm gm;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            SwarmBot target = gm.FindNextTarget();
            if (target == null)
                return;

            if (gm.distToTag > (target.transform.position - transform.position).magnitude)
            {
                //player.Untag();
                target.TagMe();
            }
        }
    }
}
