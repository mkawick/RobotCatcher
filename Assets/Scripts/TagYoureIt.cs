using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class TagYoureIt : MonoBehaviour
{
    public bool amIIt;
    public bool autoTarget = false;
    public GameManager gm;

    public Renderer modelRenderer;
    TagYoureIt target;
    internal TagYoureIt whoTaggedMeLast;

    public float lastTaggedTime;
    public float lastTimeIChangedTarget;
    public float waitTimeBeforeChangingTarget = 2.0f;
    //Time tagTimeOut;
    private void Start()
    {
        

    }
    private void Update()
    {
        if(amIIt == true && autoTarget == true)
        {
            if(DoIHaveATarget())
            {
                var control = GetComponent<AICharacterControl>();
                control.target = target.transform;
                ChooseATarget();// possibly pick a new target
            }
            else
            {
                if(HasEnoughTimeElapsed() == true)
                {
                    ChooseATarget();
                }
            }
        }
        else // todo, avoid
        {
            if (target == null)
            {
                target = gm.WhoIsIt();
            }
            RunAway();
        }
    }

    void RunAway()
    {
        if (autoTarget == false)
            return;

        if (Time.time - lastTimeIChangedTarget > waitTimeBeforeChangingTarget)
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


             SetTargetLocation(normalizedDirection);
        }
    }

    void ChooseInitialDirection()
    {
        Vector3 normalizedDirection = (target.transform.position - this.transform.position);
        normalizedDirection.y = 0;// no height
        normalizedDirection.Normalize();
        SetTargetLocation(normalizedDirection);

    }
    void SetTargetLocation(Vector3 normalizedDirection)
    {
        float dist = 5.0f;
        normalizedDirection *= dist;
        var control = GetComponent<AICharacterControl>();
        control.SetTarget(normalizedDirection);
        lastTimeIChangedTarget = Time.time;
    }

    void ChooseATarget()
    {
        if (Time.time - lastTimeIChangedTarget > waitTimeBeforeChangingTarget)
        {
            target = gm.FindNextTarget(this, whoTaggedMeLast);
            lastTimeIChangedTarget = Time.time;
        }
    }

    bool HasEnoughTimeElapsed()
    {
        return (Time.time - lastTaggedTime) > gm.tagTimeOut;
    }
    bool DoIHaveATarget()// todo check for distance
    {
        return target != null;
    }

    public void YoureIt(TagYoureIt playerWhoTouchedYou)
    {
        amIIt = true;
        gm.SetMeIt(this, amIIt);
        target = null;
        var control = GetComponent<AICharacterControl>();
        control.SetTarget(null);

        whoTaggedMeLast = playerWhoTouchedYou;

        lastTaggedTime = Time.time;
    }
    public void Untag()
    {
        amIIt = false;
        gm.SetMeIt(this, amIIt);
        target = null;
        var control = GetComponent<AICharacterControl>();
        control.SetTarget(null);

        // we want to find a new target immediately. // two seconds ago
        lastTimeIChangedTarget = Time.time - waitTimeBeforeChangingTarget;
    }
}

