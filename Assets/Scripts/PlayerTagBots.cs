using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTagBots : MonoBehaviour
{
    TagYoureIt player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<TagYoureIt>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true && player.amIIt == true)
        {
            TagYoureIt target = player.gm.FindNextTarget(player);
            player.Untag();
            target.YoureIt(player);
        }
        else
        {
            //ClearTarget();
        }
    }
}
