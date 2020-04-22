using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class SwarmBot : MonoBehaviour
{
    public bool amITagged;
    //public bool autoTarget = false;
    public GameManagerBotSwarm gm;
    public Renderer modelRenderer;
    public PlayerTagSwarmBot target;

    [HideInInspector]
    public float lastTaggedTime;
    [HideInInspector]
    public float lastTimeIChangedTarget;
    public float waitTimeBeforeChangingTarget = 2.0f;
    AICharacterControl control;
    public float speedWhenTagged = 0.4f;
    public float speedWhenRunningAway = 1.2f;
    //var control = GetComponent<AICharacterControl>();

    void Start()
    {
        UpateSpeed();
        control = GetComponent<AICharacterControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (amITagged == true)
        {
            if (ChasePlayer() == true)
            {
                if (IsPlayerCloseEnoughToTag())
                {
                    Untag();
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
        if (HasEnoughTimeExpired() &&
            (target.transform.position - this.transform.position).magnitude < gm.distToTag )
                return true;
        return false;
    }

    bool ChasePlayer()
    {
        if(HasEnoughTimeExpired())
        {
            if (target == null)
            {
                target = gm.GetPlayer();
                UpateSpeed();
                control.SetTarget(target.transform);
            }
            return true;
        }
        return false;
    }

    bool HasEnoughTimeExpired()
    {
        if (Time.time - lastTimeIChangedTarget > waitTimeBeforeChangingTarget)
        {
            return true;
        }
        return false;
    }

    void RunAway()
    {
       /* if (autoTarget == false)
            return;*/

        if (HasEnoughTimeExpired())
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
        }
    }
    void SetTargetLocation(Vector3 normalizedDirection)
    {
        float dist = 5.0f;
        normalizedDirection *= dist;
        control.SetTarget(normalizedDirection);
        lastTimeIChangedTarget = Time.time;
    }

    public void TagMe()
    {
        amITagged = true;
        gm.AmIConverted(this, amITagged);
        target = null;
        control.SetTarget(null);

        lastTaggedTime = Time.time;
    }

    void Untag()
    {
        amITagged = false;
        gm.AmIConverted(this, amITagged);
        target = null;
        control.SetTarget(null);

        lastTaggedTime = Time.time;
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
}
