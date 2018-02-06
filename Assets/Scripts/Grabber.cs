using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class Grabber : OVRGrabber {
    public float rotationSpeed;
    public float resumeDelay = 2.0f;

    private Quaternion grabbedObjOrigRotation;
    private Vector3 controllerGrabPosition;
    private bool isGrabbing;
    DataGlobe globe;
    AutoMoveAndRotate autoMover;
    float rotationX = 0.0f;
    float resumeTime = 0f;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {
                globe = hitInfo.transform.GetComponent<DataGlobe>();
                if (globe != null)
                {
                    autoMover = globe.gameObject.GetComponent<AutoMoveAndRotate>();
                    isGrabbing = true;
                }
                else
                {
                    isGrabbing = false;
                }
            }
            else
            {
                isGrabbing = false;
            }
        }
        else
        {
            isGrabbing = false;
        }

        if (isGrabbing)
        {
            resumeTime = 0f;
            autoMover.enabled = false;
            rotationX = globe.transform.eulerAngles.y - (Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime);
            globe.transform.eulerAngles = new Vector3(0, rotationX, 0);
        }
        else if (globe != null && autoMover != null)
        {
            if (resumeTime >= resumeDelay)
            {
                rotationX = 0;
                //autoMover.enabled = true;
                resumeTime = 0f;
            }
            else
            {
                resumeTime += Time.deltaTime;
            }
        }
    }

    protected override void MoveGrabbedObject(Vector3 pos, Quaternion rot, bool forceTeleport = false)
    {
        if (m_grabbedObj == null)
        {
            return;
        }
        Rigidbody grabbedRigidbody = m_grabbedObj.grabbedRigidbody;

        if ((grabbedRigidbody.constraints & RigidbodyConstraints.FreezePosition) != RigidbodyConstraints.None)
        {
            //movement is locked, rotate only            
            rotationX = grabbedObjOrigRotation.eulerAngles.y - ((OVRInput.GetLocalControllerPosition(m_controller).x - controllerGrabPosition.x) * rotationSpeed);
            grabbedRigidbody.MoveRotation(Quaternion.Euler(0, rotationX, 0));
        } else
        {
            base.MoveGrabbedObject(pos, rot, forceTeleport);
        }
    }

    protected override void GrabBegin()
    {
        base.GrabBegin();
        if (m_grabbedObj != null)
        {
            grabbedObjOrigRotation = m_grabbedObj.transform.rotation;
            controllerGrabPosition = OVRInput.GetLocalControllerPosition(m_controller);
        }
    }
}
