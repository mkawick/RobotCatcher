﻿using System;
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

    public float lastTaggedTime;
    public float lastTimeIChangedTarget;
    //Time tagTimeOut;
    private void Start()
    {
        

    }
    private void Update()
    {
        if(amIIt == true && autoTarget == true)
        {
            if(DoIHaveATarget() )
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
        }
    }

    void ChooseATarget()
    {
        if (Time.time - lastTimeIChangedTarget > 2.0f)
        {
            target = gm.FindNextTarget(this);
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

        lastTaggedTime = Time.time;
    }
    public void Untag()
    {
        amIIt = false;
        gm.SetMeIt(this, amIIt);
        target = null;
        var control = GetComponent<AICharacterControl>();
        control.SetTarget(null);
    }
}

