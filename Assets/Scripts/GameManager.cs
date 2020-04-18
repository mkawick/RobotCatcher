﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject pathContainer;

    [SerializeField]
    TagYoureIt[] tagPlayers;

    [SerializeField]
    Material[] matsForTagged;

    [SerializeField]
    internal float tagTimeOut = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        if (pathContainer != null)
        {
            pathContainer.SetActive(false);
        }
        foreach (var tagee in tagPlayers)
        {
            tagee.gm = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public TagYoureIt FindNextTarget(TagYoureIt fromPlayer)
    {
        float smallestDist = 1000;
        TagYoureIt target = null;
        foreach ( var tagee in tagPlayers)
        {
            if(tagee != fromPlayer)
            {
                float dist = (fromPlayer.transform.position - tagee.transform.position).magnitude;
                if (smallestDist > dist)
                {
                    target = tagee;
                    smallestDist = dist;
                }
            }
        }

        return target;
    }
    
    internal void SetMeIt(TagYoureIt tagged, bool isIt)
    {
        if (isIt)
            tagged.modelRenderer.material = matsForTagged[0];
        else
            tagged.modelRenderer.material = matsForTagged[1];
    }
}
