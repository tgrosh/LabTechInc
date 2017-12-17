using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour {
    public InfectionPoint hostPoint;
    public float updateInterval = 1.0f;
    public bool active;

    float currentTime = 0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (active && currentTime >= updateInterval)
        {
            //Debug.Log("Virus is updating infection levels in " + hostPoint.countryName);
            hostPoint.Infection *= 2f;
            currentTime = 0;
        }
        currentTime += Time.deltaTime;
	}
}
