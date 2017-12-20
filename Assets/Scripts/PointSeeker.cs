using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PointSeeker : MonoBehaviour {
    public float radius = .01f;
    private bool uiMode = false;
    private UI ui;

	// Use this for initialization
	void Start () {
        ui = GameObject.Find("UICanvas").GetComponent<UI>();
    }
	
	// Update is called once per frame
	void Update () {
        DataPoint point;

        if (Input.GetKeyUp(KeyCode.F1))
        {
            if (uiMode)
            {
                uiMode = false;
                ui.HideUI();
            } else
            {
                uiMode = true;
                ui.ShowUI();
            }
            
            transform.parent.GetComponentInChildren<FirstPersonController>().enabled = !uiMode;
        }

        if (Input.GetKeyUp(KeyCode.PageUp))
        {
            radius *= 2f;
        }

        if (Input.GetKeyUp(KeyCode.PageDown))
        {
            radius *= .5f;
        }

        if (!uiMode && Input.GetMouseButton(0))
        {
            RaycastHit[] hits = Physics.SphereCastAll(Camera.main.ScreenPointToRay(Input.mousePosition), radius);
            foreach (RaycastHit sphereHit in hits)
            {
                point = sphereHit.transform.GetComponent<DataPoint>();
                if (point != null)
                {
                    if (Input.GetKey(KeyCode.LeftAlt) && !string.IsNullOrEmpty(point.CountryName))
                    {
                        //do nothing
                    } else if (!Input.GetKey(KeyCode.LeftControl) && point.Populated)
                    {
                        point.Selected = true;
                    }
                    else if (Input.GetKey(KeyCode.LeftControl))
                    {
                        point.Populated = true;
                    }
                }
            }
        }
        else if (!uiMode && Input.GetMouseButton(1))
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
