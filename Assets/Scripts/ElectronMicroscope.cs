using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronMicroscope : MonoBehaviour
{
    public Canvas microscopeScreen;
    public Transform containerPlacemat;
    public Virus virus;
    public MicroscopeDoorUI doorUI;

    // Use this for initialization
    void Start () {
        microscopeScreen.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        virus = other.transform.root.GetComponent<Virus>();

        if (virus != null)
        {
            microscopeScreen.gameObject.SetActive(true);
            doorUI.Close();

            Rigidbody body = other.transform.root.GetComponent<Rigidbody>();
            body.MovePosition(containerPlacemat.position);
            body.MoveRotation(containerPlacemat.rotation);
            body.isKinematic = true;

            OVRGrabbable grabbable = other.transform.root.GetComponent<OVRGrabbable>();
            if (grabbable != null)
            {
                OVRGrabber grabber = grabbable.grabbedBy;
                if (grabber != null)
                {
                    grabber.ForceRelease(grabbable);
                }
            } 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (virus != null)
        {
            microscopeScreen.gameObject.SetActive(false);
        }
    }
}
