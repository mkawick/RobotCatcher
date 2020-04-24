using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class SwarmBot : MonoBehaviour
{
    public bool amITagged;
    //public bool autoTarget = false;
    internal GameManagerBotSwarm gm;
    public Renderer modelRenderer;
    internal PlayerTagSwarmBot target;
    internal AgentManagerBotSwarm am;

    [HideInInspector]
    public float lastTimeIStartedChasing;
    float whenCanIStartChasing;
    float whenCanIStartRunningAway;

    public float waitTimeBeforeChangingTarget = 2.0f;
    public float bumpTimeout = 0.3f;
    public float nextBumpTimePossible = 0;
    AICharacterControl control;
    public float speedWhenTagged = 0.4f;
    public float speedWhenRunningAway = 1.2f;
    internal bool isGameRunning = false;
    //var control = GetComponent<AICharacterControl>();

    void Start()
    {
        GetControl();
        UpateSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        if (amITagged == true && isGameRunning == true)
        {
            if (HasEnoughTimeExpired() == true)
            {
                ChasePlayer();
                if (IsPlayerCloseEnoughToTag() && Time.time > nextBumpTimePossible)
                {
                    nextBumpTimePossible = Time.time + bumpTimeout;
                    if (am.DoBotsReturnToUntagged == true)
                    {
                        //Debug.Log("Update::untag coming");
                        Untag();
                    }
                    gm.PlaySound(GameManagerBotSwarm.AudioClipToPlay.PlayerBumped);
                }
            }
        }
        else // todo, avoid
        {
            if (target == null)
            {
                target = gm.GetPlayer();
            }
            RunAway();
        }
    }

    bool IsPlayerCloseEnoughToTag()
    {
        if ((target.transform.position - this.transform.position).magnitude < gm.distToTag)
        {
            //Debug.Log("IsPlayerCloseEnoughToTag::true");
            return true;
        }
        //Debug.Log("IsPlayerCloseEnoughToTag::false");
        return false;
    }

    void ChasePlayer()
    {
        if (target == null)
        {
            target = gm.GetPlayer();
            UpateSpeed();
            GetControl().SetTarget(target.transform);
        }
        //Debug.Log("ChasePlayer");
    }

    bool HasEnoughTimeExpired()
    {
        if (Time.time > whenCanIStartChasing)
        {
            //Debug.Log("HasEnoughTimeExpired::true");
            return true;
        }
        //Debug.Log("HasEnoughTimeExpired::false");
        return false;
    }

    void RunAway()
    {
        //Debug.Log("RunAway");

        if (Time.time > whenCanIStartRunningAway)
        {
            Vector3 dirToTarget = (target.transform.position - this.transform.position);
            dirToTarget.y = 0;
            dirToTarget.Normalize();

            Vector3 normalizedDirection;
            do
            {
                normalizedDirection = UnityEngine.Random.onUnitSphere;
                normalizedDirection.y = 0;
                normalizedDirection.Normalize();

            } while (Vector3.Dot(dirToTarget, normalizedDirection) > 0);

            UpateSpeed();
            SetTargetLocation(normalizedDirection);
            whenCanIStartRunningAway = Time.time + 2.0f;
        }
    }
    void SetTargetLocation(Vector3 normalizedDirection)
    {
        //Debug.Log("SetTargetLocation");
        float dist = 5.0f;
        normalizedDirection *= dist;
        GetControl().SetTarget(normalizedDirection);
    }

    public void TagMe()
    {
        //Debug.Log("TagMe");
        if (isGameRunning == false)
            return;

        amITagged = true;
        gm.AmIConverted(this, amITagged);
        target = null;
        GetControl().SetTarget(null);

        whenCanIStartChasing = Time.time + 2.0f;
    }

    public void Untag()
    {
        //Debug.Log("Untag");
        if (isGameRunning == false)
            return;

        amITagged = false;
        gm.AmIConverted(this, amITagged);
        target = null;


        GetControl().SetTarget(null);
        whenCanIStartRunningAway = Time.time + 2.0f;
    }

    void UpateSpeed()
    {
        var character = GetComponent<ThirdPersonCharacter>();
        if(amITagged)
        {
            character.MoveSpeedMultiplier = 0.4f;
            character.AnimSpeedMultiplier = 0.8f;
        }
        else 
        {
            character.MoveSpeedMultiplier = 1.2f;
            character.AnimSpeedMultiplier = 1.2f;
        }
    }

    public AICharacterControl GetControl()
    {
        if (control == null)
            control = GetComponent<AICharacterControl>();
        return control;
    }
}
