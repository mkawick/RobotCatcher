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

    public float lastTaggedTime;
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
            }
            else
            {
                if(HasEnoughTimeElapsed() == true)
                { 
                    target = gm.FindNextTarget(this);
                }
            }
        }
        else // todo, avoid
        { 
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

