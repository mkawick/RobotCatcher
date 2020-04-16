using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    public Material[] friendlinessMats;
    void Start()
    {
        EnemyAgentAI [] ai = gameObject.GetComponentsInChildren<EnemyAgentAI>();
        foreach (var child in ai)
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

        EnemyAgentAI[] ai = gameObject.GetComponentsInChildren<EnemyAgentAI>();
        foreach (var child in ai)
        {
            if((child.transform.position - position).magnitude < range)
            {
                enemies.Add(child);
            }
        }

        return enemies;
    }
}
