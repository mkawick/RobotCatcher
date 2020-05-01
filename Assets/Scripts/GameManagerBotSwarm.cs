using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class GameManagerBotSwarm : MonoBehaviour
{
    int roundNumber = 1;
    

    
    [SerializeField]
    Material[] matsForTagged;

    [SerializeField]
    internal float tagTimeOut = 3.0f;

    [SerializeField]
    internal float distToTag = 1.2f;

    [SerializeField]
    float speedIncreasePerRound = 0.3f;
    [SerializeField]
    float numSecondsLostPerRound = 2f;
    [SerializeField]
    float minSecondsLostPerRound = 10;

    public bool areWeASnake = true;
    public bool fieldScalingEnabled = false;

    [SerializeField]
    PlayerTagSwarmBot player;

    [SerializeField]
    AgentManagerBotSwarm botManager;

    
    List<GameObject> listOfSnakeObjects;
    


float fieldSizeScale = 1;
    [SerializeField]
    GameObject gameField;
    [SerializeField]
    NavMeshSurface navMeshSurface;


    [SerializeField]
    Text scoreField;
    [SerializeField]
    Text youWin;
    [SerializeField]
    Text startGame;
    [SerializeField]
    Text countDown;

    //public AudioClip ac;
    public AudioSource [] audioSource;

    enum GameStates
    {
        Beginning,
        NormalPlay,
        RoundComplete
    }
    internal enum AudioClipToPlay
    {
        AgentTagged,
        PlayerBumped,
        EndGame,
        StartGame,
        TimeTick
    }
    GameStates gameState = GameStates.Beginning;
    float whenDoesNextStateChangeOccur = 0;
    float whenDidLastStateChangeOccur = 0;
    float normalRoundTime  = 60;
    int timeRemainingTickCounter = 0;

    int permScore = 0;
    public int stageNum { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        botManager.SetupAllBots(this);
        botManager.StartGame(false);
        listOfSnakeObjects = new List<GameObject>();
        stageNum = 0;

        gameState = GameStates.Beginning;
        whenDoesNextStateChangeOccur = Time.time + 2.0f;
        whenDidLastStateChangeOccur = Time.time;
        //ScaleTheField(gameField, 2);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGameUIState();
        TransitionGameState();
    }
     
    internal bool PlaySound (AudioClipToPlay clipId)
    {
        if (audioSource != null && audioSource.Length > (int) clipId)
        {
            audioSource[(int)clipId].Play();
            return true;
        }

        return false;
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
                        whenDoesNextStateChangeOccur = Time.time + normalRoundTime;// 30 seconds to finish the round

                        normalRoundTime -= numSecondsLostPerRound;
                        if (normalRoundTime <minSecondsLostPerRound)
                            normalRoundTime = minSecondsLostPerRound;

                        
                        botManager.StartGame(true);
                        PlaySound(AudioClipToPlay.StartGame);
                        botManager.MakeOpponentsFaster(speedIncreasePerRound);
                        listOfSnakeObjects.Clear();
                        listOfSnakeObjects.Add(player.gameObject);
                    }
                    break;
                case GameStates.NormalPlay:
                    {
                        gameState = GameStates.RoundComplete;
                        whenDoesNextStateChangeOccur = Time.time + 5.0f;
                        stageNum++;
                        botManager.StartGame(false);
                        PlaySound(AudioClipToPlay.EndGame);
                        ScaleTheField(gameField, 0.2f);
                    }
                    break;
                case GameStates.RoundComplete:
                    {
                        roundNumber++;
                        gameState = GameStates.Beginning;
                        whenDoesNextStateChangeOccur = Time.time + 4.0f;
                        RebuildNavMesh();
                    }
                    break;
            }
        }
    }

    void ScaleTheField(GameObject field, float percentage = 0.1f)
    {
        if (fieldScalingEnabled == false)
            return;
        if (field != null)
        {
            float newFieldScale = fieldSizeScale * (1.0f + percentage);
            fieldSizeScale = newFieldScale;
            Vector3 scale = field.transform.localScale;
            scale *= fieldSizeScale;
            iTween.ScaleTo(gameField, scale, 4);
        }
    }
    void RebuildNavMesh()
    {
        var settings = navMeshSurface.GetBuildSettings();

        settings.agentRadius = 0.5f;
        settings.agentHeight = 2.0f;
        settings.agentSlope = 23;
        settings.agentClimb = 0.4f;
        settings.minRegionArea = 2.0f;

        navMeshSurface.BuildNavMesh();
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
        if(countDown != null)
        {
            if (gameState == GameStates.NormalPlay)
            {
                countDown.gameObject.SetActive(true);
                int secondsLeft = (int)(whenDoesNextStateChangeOccur - Time.time + 0.5);
                countDown.text = "Seconds left: " + secondsLeft;// rounding
                if (secondsLeft != timeRemainingTickCounter)
                {
                    timeRemainingTickCounter = secondsLeft;
                    //PlaySound(AudioClipToPlay.TimeTick);
                }
            }
            else
            {
                countDown.gameObject.SetActive(false);
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
        {
            tagged.modelRenderer.material = matsForTagged[0];
            if(areWeASnake)
            {
                listOfSnakeObjects.Add(tagged.gameObject);
            }
        }
        else
        {
            tagged.modelRenderer.material = matsForTagged[1];
        }
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
        //return;
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

    public GameObject GetAIToFollow(GameObject whoWantsToKnow)
    {
        GameObject go = null;
        foreach(var search in listOfSnakeObjects)
        {
            if (search == whoWantsToKnow)// always return the previous guy
                return go;
            go = search;
        }
        return go ;
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
