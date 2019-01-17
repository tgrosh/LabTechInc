using ControllerSelection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPointSeeker : MonoBehaviour {
    private OVRInput.Controller activeController;
    private Transform trackingSpace;
    private IcoGlobe icoGlobe;
    private int currentTriangle;

    // Use this for initialization
    void Start () {
        trackingSpace = OVRInputHelpers.FindTrackingSpace();
    }
	
	// Update is called once per frame
	void Update () {
        activeController = OVRInputHelpers.GetControllerForButton(OVRInput.Button.PrimaryIndexTrigger, activeController);
        Ray pointer = OVRInputHelpers.GetSelectionRay(activeController, trackingSpace);
        RaycastHit hit; // Was anything hit?
        if (!Physics.Raycast(pointer, out hit, 10))
            return;

        icoGlobe = hit.collider.transform.parent.GetComponent<IcoGlobe>();

        if (icoGlobe == null) return;
        
        icoGlobe.Hover(hit.triangleIndex);

        //Mesh mesh = meshCollider.sharedMesh;
        //Vector3[] vertices = mesh.vertices;
        //Color[] colors = mesh.colors;
        //int[] triangles = mesh.triangles; 
        
        //if (colors.Length == 0)
        //{
        //    colors = new Color[vertices.Length]; //just incase
        //}

        //colors[triangles[hit.triangleIndex * 3 + 0]] =
        //    colors[triangles[hit.triangleIndex * 3 + 1]] =
        //    colors[triangles[hit.triangleIndex * 3 + 2]] = Color.red;

        //mesh.colors = colors;

        //Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        //Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        //Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];

        //Transform hitTransform = hit.collider.transform;
        //p0 = hitTransform.TransformPoint(p0);
        //p1 = hitTransform.TransformPoint(p1);
        //p2 = hitTransform.TransformPoint(p2);

        //Debug.DrawLine(p0, p1);
        //Debug.DrawLine(p1, p2);
        //Debug.DrawLine(p2, p0);
    }
}
