using UnityEngine;

public class ElectronMicroscope : MonoBehaviour
{
    public MicroscopeUI microscopeUI;
    public Transform containerPlacemat;
    public Virus virus;
    public MicroscopeDoorUI doorUI;

    // Use this for initialization
    void Start () {
        microscopeUI.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        virus = other.transform.root.GetComponent<Virus>();

        if (virus != null)
        {
            microscopeUI.gameObject.SetActive(true);
            microscopeUI.virus = virus;
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
            microscopeUI.virus = null;
            microscopeUI.gameObject.SetActive(false);
        }
    }
}
