using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerBotSwarm : MonoBehaviour
{
   /* [SerializeField]
    TagYoureIt[] tagPlayers;*/

    [SerializeField]
    Material[] matsForTagged;

    [SerializeField]
    internal float tagTimeOut = 3.0f;

    [SerializeField]
    internal float distToTag = 1.1f;

    [SerializeField]
    PlayerTagSwarmBot player;

    [SerializeField]
    SwarmBot[] bots;

    [SerializeField]
    Text scoreField;

    [SerializeField]
    Text youWin;

    bool gameState = true;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var tagee in bots)
        {
            tagee.gm = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void AmIConverted(SwarmBot tagged, bool isConverted)
    {
        if (gameState == false)
            return;
        if (isConverted)
            tagged.modelRenderer.material = matsForTagged[0];
        else
            tagged.modelRenderer.material = matsForTagged[1];
        UpdateScore();
    }



    void UpdateScore()
    {
        if (scoreField)
        {
            int num = 0;
            foreach (var tagee in bots)
            {
                if (tagee.amITagged == true)
                    num++;
            }
            scoreField.text = "Score: " + num;
            EvalGameState(num);
        }
        
    }

    void EvalGameState( int numTagged)
    {
        if (numTagged == bots.Length)
        {
             gameState = false;
        }

        if(gameState == true)
        {
            youWin.gameObject.SetActive(false);
        }
        else
        {
            youWin.gameObject.SetActive(true);
        }
    }

    public PlayerTagSwarmBot GetPlayer()
    {
        return player;
    }
    public SwarmBot FindNextTarget()
    {
        float smallestDist = 1000;
        SwarmBot target = null;
        foreach (var tagee in bots)
        {
            if (tagee.amITagged == true)
                continue;

            float dist = (player.transform.position - tagee.transform.position).magnitude;
            if (smallestDist > dist)
            {
                target = tagee;
                smallestDist = dist;
            }
        }

        return target;
    }
}
