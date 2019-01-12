using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PointSeeker : MonoBehaviour {
    public float radius = .01f;
    public Material gizmoMaterial;
    public Camera firstPersonCamera;

    private UI ui;
    string prevCountryName = "";
    private GameObject sphere;

	// Use this for initialization
	void Start () {
        ui = GameObject.Find("UICanvas").GetComponent<UI>();
    }
        
    // Update is called once per frame
    void Update () {
        DataPoint point;
        
        if (Input.GetKeyUp(KeyCode.PageUp) || Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            radius *= 2f;
        }

        if (Input.GetKeyUp(KeyCode.PageDown) || Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            radius *= .5f;
        }

        if (!ui.uiPanel.activeInHierarchy)
        {
            RaycastHit hit;
            if (Physics.Raycast(firstPersonCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                DrawSphere(hit.point, radius);
            }

            if (Input.GetMouseButton(0))
            {
                RaycastHit[] hits = Physics.SphereCastAll(firstPersonCamera.ScreenPointToRay(Input.mousePosition), radius);
                foreach (RaycastHit sphereHit in hits)
                {
                    point = sphereHit.transform.GetComponent<DataPoint>();
                    if (point != null)
                    {
                        if (Input.GetKey(KeyCode.LeftAlt) &&
                            (
                            (ui.editMode == EditMode.COUNTRIES && !string.IsNullOrEmpty(point.CountryName)) ||
                            (ui.editMode == EditMode.REGIONS && (prevCountryName != "" && prevCountryName != point.CountryName)) ||
                            (ui.editMode == EditMode.HEALTHCARE && point.HealthCare != 0))
                            )
                        {
                            //do nothing
                        }
                        else if (!Input.GetKey(KeyCode.LeftControl) && point.Populated)
                        {
                            point.Selected = true;
                            prevCountryName = point.CountryName;
                        }
                        else if (Input.GetKey(KeyCode.LeftControl))
                        {
                            point.Populated = true;
                        }
                    }
                }
            }
            else if (Input.GetMouseButton(1))
            {
                RaycastHit[] hits = Physics.SphereCastAll(firstPersonCamera.ScreenPointToRay(Input.mousePosition), radius);
                foreach (RaycastHit sphereHit in hits)
                {
                    point = sphereHit.transform.GetComponent<DataPoint>();
                    if (point != null)
                    {
                        if (!Input.GetKey(KeyCode.LeftControl) && point.Populated)
                        {
                            point.Selected = false;
                        }
                        else if (Input.GetKey(KeyCode.LeftControl))
                        {
                            point.Populated = false;
                        }
                    }
                }
            }
        }
    }

    private void DrawSphere(Vector3 position, float radius)
    {
        if (sphere == null)
        {
            sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Destroy(sphere.GetComponent<SphereCollider>());
            sphere.GetComponent<MeshRenderer>().material = gizmoMaterial;
        }
        sphere.transform.position = position;
        sphere.transform.localScale = Vector3.one * radius * 2;
    }

    private Vector3 GetAverageHitPoint(RaycastHit[] hits)
    {
        if (hits.Length == 0) return Vector3.zero;

        float x = 0f;
        float y = 0f;
        float z = 0f;
        foreach (RaycastHit hit in hits)
        {
            Vector3 pos = hit.point;
            x += pos.x;
            y += pos.y;
            z += pos.z;
        }
        return new Vector3(x / hits.Length, y / hits.Length, z / hits.Length);
    }
}
