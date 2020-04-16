using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerKeyboardLocomotion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var control = GetComponent<ThirdPersonCharacter>();
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = v * Vector3.forward + h * Vector3.right;
        control.Move(moveDir, false, false);
        /*   if (Input.GetKeyDown(KeyCode.UpArrow) == true)
           {
               moveDir = new Vector3(0, 0, 1);
               control.Move(moveDir, false, false);
           }
           if (Input.GetKeyDown(KeyCode.LeftArrow) == true)
           {
               moveDir = new Vector3(1, 0, 0);
               control.Move(moveDir, false, false);
           }*/
    }
}
