using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneListener : MonoBehaviour {
    public Airplane airplane;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeoffComplete()
    {
        airplane.TakeoffComplete();
    }

    public void LandingComplete()
    {
        airplane.LandingComplete();       
    }
}
