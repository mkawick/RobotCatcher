using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class BotAttacks : MonoBehaviour
{
    TagYoureIt bot;
    AICharacterControl aiController;
    // Start is called before the first frame update
    void Start()
    {
        bot = GetComponent<TagYoureIt>();
        aiController = GetComponent<AICharacterControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bot.amIIt == true && aiController.target != null)
        {
            if((aiController.target.position - this.transform.position).magnitude < 2)
            {
                TagYoureIt tagger = aiController.target.gameObject.GetComponent<TagYoureIt>();
                bot.Untag();
                tagger.YoureIt(bot);
            }
        }
        else
        {
            //ClearTarget();
        }
    }
}
