using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distributor : MonoBehaviour {
    public DistributorUI distributorScreen;
    public Transform containerPlacemat;
    public Virus virus;

	// Use this for initialization
	void Start () {
        distributorScreen.EnableDeployment(false);
    }
	
    void OnTriggerEnter(Collider other)
    {
        virus = other.transform.root.GetComponent<Virus>();

        if (virus != null)
        {
            distributorScreen.EnableDeployment(true);

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
            distributorScreen.EnableDeployment(false);
        }
    }
}
