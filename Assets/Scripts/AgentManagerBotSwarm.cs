using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManagerBotSwarm : MonoBehaviour
{
    GameManagerBotSwarm gm;
    [SerializeField]
    SwarmBot[] bots;

    [SerializeField]
    SwarmBot[] laterStagebots;

    public bool DoBotsReturnToUntagged = true;
    public float runAwayDistanceMultiplier = 5.0f;
    public float runAwayDistMultChangePerRound = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetGame()
    {
        foreach (var tagee in bots)
        {
            tagee.amITagged = false;
            tagee.isGameRunning = false;
        }
        ActivateBotsForStage(0);
    }

    private void ActivateBotsForStage(int stageNum)
    {
        foreach (var tagee in bots)
        {
            if (tagee.stageBegins <= stageNum)
                tagee.gameObject.SetActive(true);
            else
                tagee.gameObject.SetActive(false);
        }
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
        else
        {
            ActivateBotsForStage(gm.stageNum);            
        }
    }

    public void SetupAllBots(GameManagerBotSwarm _gm)
    {
        gm = _gm;
        foreach (var tagee in bots)
        {
            tagee.gm = gm;
            tagee.am = this;
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
        int count = 0;
        foreach (var tagee in bots)
        {
            if(tagee.stageBegins <= gm.stageNum)
            count++;
        }
        return count;
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
            runAwayDistanceMultiplier += runAwayDistMultChangePerRound;
}
    }
}
