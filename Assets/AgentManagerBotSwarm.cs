using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManagerBotSwarm : MonoBehaviour
{
    [SerializeField]
    SwarmBot[] bots;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame(bool begin = true)
    {
        foreach (var tagee in bots)
        {
            tagee.isGameRunning = begin;
        }
        if(begin == true)
        {
            foreach (var tagee in bots)
            {
                tagee.Untag();
            }
        }
    }

    public void SetupAllBots(GameManagerBotSwarm gm)
    {
        foreach (var tagee in bots)
        {
            tagee.gm = gm;
        }
    }

    public int GetNumTaggedBots()
    {
        int num = 0;
        foreach (var tagee in bots)
        {
            if (tagee.amITagged == true)
                num++;
        }
        return num;
    }
    public int GetNumBots()
    {
        return bots.Length;
    }

    public SwarmBot[] GetBots()
    {
        return bots;
    }

    public void MakeOpponentsFaster(float amount)
    {
        foreach (var tagee in bots)
        {
            tagee.GetControl().character.ForwardSpeedMultiplier += amount;
        }
    }
}
