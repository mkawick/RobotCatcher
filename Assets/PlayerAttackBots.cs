using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBots : MonoBehaviour
{
    EnemyAgentAI target;
    public AgentManager agentManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            SelectTarget();
            TryToAttackTarget();
        }
        else
        {
            ClearTarget();
        }
    }

    void SelectTarget()
    {
        if (target != null)
            return;
        List<EnemyAgentAI> enemies = agentManager.GetListOfNearbyEnemies(transform.position, 4.0f);
        if (enemies.Count == 0)
            return;
        // todo, check player facing
        target = enemies[0];
    }
    void TryToAttackTarget()
    {
        target.ChangeFriendlinessToPlayer(true);
    }
    void ClearTarget()
    {
        target = null;
    }
}
