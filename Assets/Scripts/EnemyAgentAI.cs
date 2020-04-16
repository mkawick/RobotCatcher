using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public abstract class IAgentMovement
{
    public EnemyAgentAI myAgent;
    int target;
    protected float lastSwitchTime;
    public float minSwitchTime = 3.0f;
    //bool didChangeFromPursuitToPatrol = false;

    public abstract void Reset();
    public abstract bool Update();
    protected bool HasEnoughTimeElapsedToChangeModes()
    {
        float elapsedTime = (Time.time - lastSwitchTime);
        return elapsedTime > minSwitchTime;
    }
}
public class MoveToNode : IAgentMovement
{
    
    public override void Reset()
    {
        //lastSwitchTime = Time.time;
    }
    public override bool Update()
    {
        if(IsPlayerCloseEnoughToFollow() == true &&
            HasEnoughTimeElapsedToChangeModes() == true)
        {
            lastSwitchTime = Time.time;
            return false;
        }
        if (myAgent.IsCloseToNavNode(myAgent.navNodeIndex) == true)
        {
            myAgent.navNodeIndex = myAgent.GetNextIndex(myAgent.navNodeIndex);
        }
        else if (myAgent.navNodes.Length > 0)
        {
            var control = myAgent.GetComponent<AICharacterControl>();
            control.target = myAgent.navNodes[myAgent.navNodeIndex];
        }
        return true;
    }

    bool IsPlayerCloseEnoughToFollow()
    {
        Transform[] navNodes = myAgent.navNodes;
        int navNodeIndex = myAgent.navNodeIndex;
        Transform transform = myAgent.transform;
        float distToPlayerNode = (myAgent.playerNode.position - transform.position).magnitude;

        bool isPlayerCloseEnoughToChase = distToPlayerNode <= myAgent.ClosePlayerDist;

        return isPlayerCloseEnoughToChase;
    }
}

public class MoveToPlayer : IAgentMovement
{
    public override void Reset()
    {
        //lastSwitchTime = Time.time;
    }
    public override bool Update()
    {
        if (IsPlayerFarEnoughToBeginPatrol() == true &&
             HasEnoughTimeElapsedToChangeModes() == true) 
        {
            lastSwitchTime = Time.time;
            return false;
        }
        var control = myAgent.GetComponent<AICharacterControl>();
        //if (control.target != myAgent.playerNode)
        {
            //lastSwitchTime = Time.time;
            control.target = myAgent.playerNode;// stay on target
        }

        return true;
    }

    bool IsPlayerFarEnoughToBeginPatrol()
    {
        Transform[] navNodes = myAgent.navNodes;
        int navNodeIndex = myAgent.navNodeIndex;
        Transform transform = myAgent.transform;
        float distToNavNode = (navNodes[navNodeIndex].transform.position - transform.position).magnitude;
        float distToPlayerNode = (myAgent.playerNode.position - transform.position).magnitude;

        bool isPlayerCloseEnoughToChase = distToPlayerNode <= myAgent.ClosePlayerDist;
        if (distToNavNode > myAgent.NodeDistMax)
            return true;
        if (distToNavNode < myAgent.NodeDistMin)
            return false;

        return isPlayerCloseEnoughToChase;
    }
}

//------------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------

public class EnemyAgentAI : MonoBehaviour
{

    public bool isFriendly = false;
    public Transform[] navNodes;
    public Transform playerNode;
    public Renderer modelRenderer;
    public AgentManager manager;

    public int navNodeIndex = 0;
    public int navNodeDirection = 1;

    public float ClosePlayerDist = 5;
    public float NodeDistMin = 8;
    public float NodeDistMax = 11;

    IAgentMovement[] movementModes;
    IAgentMovement currentMovementNode;
    int currentMovementIndex;

    enum NavigationMode
    {
        MovingToNextMode,
        FollowingPlayer
    }
    NavigationMode mode = NavigationMode.MovingToNextMode;
    // Start is called before the first frame update
    void Start()
    {
        movementModes = new IAgentMovement[2];
        movementModes[0] = new MoveToNode();
        movementModes[0].myAgent = this;
        movementModes[1] = new MoveToPlayer();
        movementModes[1].myAgent = this;

        currentMovementNode = movementModes[0];
    }


    public void ChangeFriendlinessToPlayer(bool isFriendlyToPlayer)
    {
        if (isFriendly != isFriendlyToPlayer)
        {
            isFriendly = isFriendlyToPlayer;
            var control = GetComponent<AICharacterControl>();
            control.target = null;

            if (manager.friendlinessMats != null &&
                manager.friendlinessMats.Length != 0 &&
                modelRenderer != null)
            {
                if (isFriendly == false) // enemy
                    modelRenderer.material = manager.friendlinessMats[0];
                else if (isFriendly == true) // friend
                    modelRenderer.material = manager.friendlinessMats[1];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isFriendly == true)
        {
            NavigateToNearbyEnemy();
            
        }
        else
        {
            DoEnemyNavigation();
        }
    }

    void DoEnemyNavigation()
    {
        if(currentMovementNode.Update() == false)
        {
            if (currentMovementIndex == 1)
                currentMovementIndex = 0;
            else
                currentMovementIndex = 1;
            currentMovementNode = movementModes[currentMovementIndex];
            currentMovementNode.Reset();
        }
       /* if (SwitchedToPlayerNode() == true)
        {
            return;
        }
        if(IsCloseToNavNode(navNodeIndex) == true)
        {
            navNodeIndex = GetNextIndex(navNodeIndex);
        }
        else if(navNodes.Length > 0)
        {
            var control = GetComponent<AICharacterControl>();
            control.target = navNodes[navNodeIndex];
        }*/
    }

    void NavigateToNearbyEnemy()
    {

    }

    bool SwitchedToPlayerNode()
    {
        if (IsCloseToPlayer() == true)
        {
            float distToNavNode = (navNodes[navNodeIndex].transform.position - transform.position).magnitude;
            float distToPlayerNode = (playerNode.position - transform.position).magnitude;
            var control = GetComponent<AICharacterControl>();
            if (IsTooFarFromNavNode(navNodeIndex) == false)
            {
                // we are in the region where we may need to switch targets.
                if (control.target == playerNode)
                {
                    if (distToNavNode > NodeDistMin)
                    {
                        control.target = navNodes[navNodeIndex].transform;
                    }
                }
                else
                {
                    if (distToPlayerNode < distToNavNode)
                    {
                        control.target = playerNode;
                    }
                }
                return true;
            }
        }
        return false;
    }
    private bool IsTooFarFromNavNode(int index)
    {
        float dist = (navNodes[index].transform.position - transform.position).magnitude;
        if (dist > NodeDistMin)
            return true;
        return false;
    }

    private bool IsCloseToPlayer()
    {
        if (playerNode == null)
            return false;

        float dist = (playerNode.position - transform.position).magnitude;
        if (dist < ClosePlayerDist)
            return true;
        return false;
    }

    public  int GetNextIndex(int index)
    {
        index += navNodeDirection;
        if (index < 0)
        {
            navNodeDirection = 1;
            index = 0;
        }
        if (index >= navNodes.Length)
        {
            
            navNodeDirection = -1;
            index = navNodes.Length - 1;
        }

        //Debug.Log("next index = " + index);
        return index;
    }

    public bool IsCloseToNavNode(int index)
    {
        if (navNodes.Length == 0)
            return false;
        if (index < 0)
            index = 0;
        if (index >= navNodes.Length)
            index = navNodes.Length - 1;

        float dist = (navNodes[index].transform.position - transform.position).magnitude;
        if (dist < 1)
            return true;
        return false;
    }
}
