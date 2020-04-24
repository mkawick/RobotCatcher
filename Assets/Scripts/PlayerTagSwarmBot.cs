using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTagSwarmBot : MonoBehaviour
{
    [SerializeField]
    GameManagerBotSwarm gm;

    [SerializeField]
    float  angleToTagInDegrees = 45;
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

    private void OnCollisionEnter(Collision collision)
    {
        SwarmBot target = collision.gameObject.GetComponent<SwarmBot>();
        if(target != null)
        {
            Vector3 dirToBot = target.transform.position - transform.position;
            dirToBot.y = 0;// remove any height differences
            float angle = Vector3.Angle(dirToBot, transform.forward);
            if(angle < angleToTagInDegrees && target.amITagged == false)
            {
                target.TagMe();
                gm.PlaySound(GameManagerBotSwarm.AudioClipToPlay.AgentTagged);
            }
        }
    }
}
