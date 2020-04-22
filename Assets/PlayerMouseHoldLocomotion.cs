using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerMouseHoldLocomotion : MonoBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        bool isButtonHeld = Input.GetMouseButton(0);

        if (isButtonHeld)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit))
            {
                return;
            }

            Vector3 direction = (hit.point - this.transform.position).normalized;
            //Vector3 moveDir = v * Vector3.forward + h * Vector3.right;

            var control = GetComponent<ThirdPersonCharacter>();
            control.MoveSpeedMultiplier = 3;
            control.AnimSpeedMultiplier = 8;
            control.Move(direction, false, false);
        }

        /*     if (isButtonHeld == false)
             {
                 if (setTargetOn != null)
                 {
                     setTargetOn.SendMessage("SetTarget", null);
                 }
                 return;
             }
             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             RaycastHit hit;
             if (!Physics.Raycast(ray, out hit))
             {
                 return;
             }
             ShowTarget(true); ;
             Vector3 worldPosition = hit.point + hit.normal * surfaceOffset;
             if (setTargetOn != null)
             {
                 setTargetOn.SendMessage("SetTarget", transform);
             }*/
    }

    internal void ShowTarget(bool show)
    {
        return;
        if (show == true)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = Vector3.zero;
        }
    }

}
