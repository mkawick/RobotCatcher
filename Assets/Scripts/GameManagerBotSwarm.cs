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
    AgentManagerBotSwarm botManager;

    int roundNumber = 1;



    [SerializeField]
    Text scoreField;
    [SerializeField]
    Text youWin;
    [SerializeField]
    Text startGame;

    enum GameStates
    {
        Beginning,
        NormalPlay,
        RoundComplete
    }
    GameStates gameState = GameStates.Beginning;
    float whenDoesNextStateChangeOccur = 0;
    float whenDidLastStateChangeOccur = 0;

    int permScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        botManager.SetupAllBots(this);
        botManager.StartGame(false);

        gameState = GameStates.Beginning;
        whenDoesNextStateChangeOccur = Time.time + 2.0f;
        whenDidLastStateChangeOccur = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGameUIState();
        TransitionGameState();
    }


    void TransitionGameState()
    { 
        if(Time.time >= whenDoesNextStateChangeOccur)
        {
            whenDidLastStateChangeOccur = Time.time;
            switch(gameState)
            {
                case GameStates.Beginning:
                    {
                        gameState = GameStates.NormalPlay;
                        whenDoesNextStateChangeOccur = Time.time + 30.0f;// 30 seconds to finish the round
                        botManager.StartGame(true);
                    }
                    break;
                case GameStates.NormalPlay:
                    {
                        gameState = GameStates.RoundComplete;
                        whenDoesNextStateChangeOccur = Time.time + 5.0f;
                        botManager.StartGame(false);
                    }
                    break;
                case GameStates.RoundComplete:
                    {
                        roundNumber++;
                        gameState = GameStates.Beginning;
                        whenDoesNextStateChangeOccur = Time.time + 4.0f;
                    }
                    break;
            }
        }
        //gameState = GameStates.RoundComplete;
    }

    void UpdateGameUIState()
    {
        if (youWin != null)
        {
            if (gameState == GameStates.RoundComplete)
            {
                youWin.gameObject.SetActive(true);
            }
            else
            {
                youWin.gameObject.SetActive(false);
            }
        }
        if (startGame != null)
        {
            if (gameState == GameStates.Beginning)
            {
                startGame.gameObject.SetActive(true);
                startGame.text = "Round " + roundNumber;
            }
            else
            {
                startGame.gameObject.SetActive(false);
            }
        }
        if (gameState == GameStates.NormalPlay)
        {
            UpdateScore();
        }
    }

    internal void AmIConverted(SwarmBot tagged, bool isConverted)
    {
        if (gameState != GameStates.NormalPlay)
            return;

        if (isConverted)
            tagged.modelRenderer.material = matsForTagged[0];
        else
            tagged.modelRenderer.material = matsForTagged[1];
    }



    void UpdateScore()
    {
        if (scoreField)
        {
            int numTagged = botManager.GetNumTaggedBots();
            int numBots = botManager.GetNumBots();
            scoreField.text = "Score: " + (numTagged + permScore);
            EvalGameState(numTagged, numBots);
        }
    }

    void EvalGameState(int numTagged, int numBots)
    {
        if (numTagged == numBots)
        {
            whenDoesNextStateChangeOccur = Time.time;
            
            permScore += numTagged;
            TransitionGameState();
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
        SwarmBot[] bots = botManager.GetBots();
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
