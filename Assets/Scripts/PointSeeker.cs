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
        ui = GameObject.Find("Canvas").GetComponent<UI>();
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;

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

        if (Input.GetMouseButton(0))
        {
            RaycastHit[] hits = Physics.SphereCastAll(Camera.main.ScreenPointToRay(Input.mousePosition), radius);
            foreach (RaycastHit sphereHit in hits)
            {
                hit = sphereHit;
                if (hit.transform.GetComponent<DataPoint>() != null)
                {
                    hit.transform.GetComponent<DataPoint>().selected = true;
                }
            }
        }
        else if (Input.GetMouseButton(1))
        {
            RaycastHit[] hits = Physics.SphereCastAll(Camera.main.ScreenPointToRay(Input.mousePosition), radius);
            foreach (RaycastHit sphereHit in hits)
            {
                hit = sphereHit;
                if (hit.transform.GetComponent<DataPoint>() != null)
                {
                    hit.transform.GetComponent<DataPoint>().selected = false;
                }
            }
        }
    }
}
