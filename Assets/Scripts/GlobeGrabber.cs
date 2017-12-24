using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class GlobeGrabber : MonoBehaviour {
    public float rotationSpeed;
    public float resumeDelay = 2.0f;

    private bool isGrabbing;
    DataGlobe globe;
    AutoMoveAndRotate autoMover;
    float rotationX = 0.0f;
    float resumeTime = 0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
                } else
                {
                    isGrabbing = false;
                }
            } else
            {
                isGrabbing = false;
            }                            
        } else
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
                autoMover.enabled = true;
                resumeTime = 0f;
            } else
            {
                resumeTime += Time.deltaTime;
            }            
        }
    }
}
