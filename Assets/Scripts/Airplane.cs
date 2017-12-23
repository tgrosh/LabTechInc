using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour {    
    public InfectionPoint source;
    public InfectionPoint destination;
    public DataGlobe globe;
    public Animator planeAnimator;

    // Use this for initialization
    void Start () {
        SetPosition(source);
        planeAnimator.SetTrigger("Takeoff");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeoffComplete()
    {
        Debug.Log("Airplane has taken off");
        SetPosition(destination);
    }

    public void LandingComplete()
    {
        Debug.Log("Airplane has landed and infected " + destination.countryName);
        destination.Infect(source.virus);
        Destroy(gameObject);
    }

    private void SetPosition(InfectionPoint point)
    {
        transform.localPosition = globe.GetGlobePosition(point.x, point.y) * 1.01f;
        transform.localRotation = Quaternion.LookRotation(transform.localPosition, globe.Earth.transform.up);
        transform.Rotate(new Vector3(0, 90, 0));
    }
}
