using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PointSeeker : MonoBehaviour {
    public float radius = .01f;
    private UI ui;
    string prevCountryName = "";

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

        if (!ui.uiPanel.activeInHierarchy && Input.GetMouseButton(0))
        {
            RaycastHit[] hits = Physics.SphereCastAll(Camera.main.ScreenPointToRay(Input.mousePosition), radius);
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
        else if (!ui.uiPanel.activeInHierarchy && Input.GetMouseButton(1))
        {
            RaycastHit[] hits = Physics.SphereCastAll(Camera.main.ScreenPointToRay(Input.mousePosition), radius);
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
