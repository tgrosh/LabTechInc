using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placemat : MonoBehaviour {
    
    public event PlacematEnterEventHandler OnPlacematEnter;
    public delegate void PlacematEnterEventHandler(GameObject placedObject);

    public event PlacematExitEventHandler OnPlacematExit;
    public delegate void PlacematExitEventHandler(GameObject placedObject);

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.GetComponent<Placeable>() == null) return;

        Rigidbody body = other.transform.root.GetComponent<Rigidbody>();
        body.position = (transform.position);
        body.rotation = (transform.rotation);
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

        if (OnPlacematEnter != null)
        {
            OnPlacematEnter(other.transform.root.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.root.GetComponent<Placeable>() == null) return;

        if (OnPlacematExit != null)
        {
            OnPlacematExit(other.transform.root.gameObject);
        }
    }
}
