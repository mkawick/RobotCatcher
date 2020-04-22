using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseHoldLocomotion : MonoBehaviour
{
    public float surfaceOffset = 1.5f;
    public GameObject setTargetOn;

    // Update is called once per frame
    private void Update()
    {
        bool isButtonHeld = Input.GetMouseButton(0);
        if (isButtonHeld == false)
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
        //transform.position = 
        if (setTargetOn != null)
        {
            setTargetOn.SendMessage("SetTarget", transform);
        }
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
