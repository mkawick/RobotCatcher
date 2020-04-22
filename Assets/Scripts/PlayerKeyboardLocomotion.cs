using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.SceneUtils;

public class PlayerKeyboardLocomotion : MonoBehaviour
{
    [SerializeField]
    PlaceTargetWithMouse mouseClick;
    public bool enableClickToMove = true;

    [SerializeField]
    float speedMultiplier = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        if (enableClickToMove)
        {
            mouseClick.enabled = true;
        }
        else
        {
            mouseClick.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var control = GetComponent<ThirdPersonCharacter>();
        float h = Input.GetAxis("Horizontal") * speedMultiplier;
        float v = Input.GetAxis("Vertical") * speedMultiplier;

        float epsilon = 0.00001f;
        if (Mathf.Abs(h) > epsilon || Mathf.Abs(v) > epsilon)
        {
            Vector3 moveDir = v * Vector3.forward + h * Vector3.right;
            control.Move(moveDir, false, false);

            Debug.Log("v= " + v + ", h= " + h);
            ClearExistingTarget();
        }
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


    void ClearExistingTarget()
    {
        mouseClick.enabled = false;
        this.SendMessage("SetTarget", this.transform);
        if (mouseClick)
            mouseClick.ShowTarget(false);
    }
}
