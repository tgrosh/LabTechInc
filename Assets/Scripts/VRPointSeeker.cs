using ControllerSelection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPointSeeker : MonoBehaviour {
    public OVRPointerVisualizer visualizer;
    private OVRInput.Controller activeController;
    private Transform trackingSpace;
    private IcoGlobe icoGlobe;
    private int currentTriangle;

    private bool isEnabled;
    private OVRPhysicsRaycaster raycaster;
    private bool grabbing;
    private float rotateX;
    private float rotateY;

    // Use this for initialization
    void Start () {
        trackingSpace = OVRInputHelpers.FindTrackingSpace();
        raycaster = GetComponentInChildren<OVRPhysicsRaycaster>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        activeController = OVRInputHelpers.GetControllerForButton(OVRInput.Button.PrimaryIndexTrigger, activeController);

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, activeController))
        {
            isEnabled = true;
        }

        raycaster.enabled = isEnabled;
        visualizer.gameObject.SetActive(isEnabled);
        if (!isEnabled) return;

        Ray pointer = OVRInputHelpers.GetSelectionRay(activeController, trackingSpace);
        RaycastHit hit; // Was anything hit?
        if (!Physics.Raycast(pointer, out hit, 10)) return;

        icoGlobe = hit.collider.transform.parent.GetComponent<IcoGlobe>();
        if (icoGlobe == null) return;

        if (activeController == OVRInput.Controller.RTouch)
        {
            icoGlobe.Hover(hit.triangleIndex);

            if (OVRInput.Get(OVRInput.RawButton.A, activeController))
            {
                icoGlobe.Select(hit.triangleIndex);
            }
        } else if (activeController == OVRInput.Controller.LTouch)
        {
            grabbing = OVRInput.Get(OVRInput.RawButton.LIndexTrigger, activeController);
        }

        if (grabbing)
        {
            rotateX = OVRInput.GetLocalControllerVelocity(activeController).x;
            icoGlobe.transform.eulerAngles = new Vector3(0, icoGlobe.transform.eulerAngles.y - (rotateX * 150 * Time.deltaTime), 0);
        }        
    }
}
