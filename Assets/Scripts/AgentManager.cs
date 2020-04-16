using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    public Material[] friendlinessMats;
    EnemyAgentAI[] aiAgents;

    public bool AreAllAiAgentsFriendly()
    {
        foreach(var ai in aiAgents)
        {
            if (ai.isFriendly == false)
                return false;
        }
        return true;    
    }
    void Start()
    {
        aiAgents = gameObject.GetComponentsInChildren<EnemyAgentAI>();
        foreach (var child in aiAgents)
        {
            //child is your child transform
            child.manager = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<EnemyAgentAI> GetListOfNearbyEnemies(Vector3 position, float range)
    {
        List<EnemyAgentAI> enemies = new List<EnemyAgentAI>();

        foreach (var ai in aiAgents)
        {
            if((ai.transform.position - position).magnitude < range)
            {
                enemies.Add(ai);
            }
        }

        return enemies;
    }
}
