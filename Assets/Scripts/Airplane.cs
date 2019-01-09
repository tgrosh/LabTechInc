using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Airplane : MonoBehaviour {    
    public InfectionPoint source;
    public InfectionPoint destination;
    public DataGlobe globe;
    public Animator planeAnimator;
    public int flightNumber;
    private Virus virus;

    public static event InfectedAirplaneTakeoffEventHandler OnInfectedAirplaneTakeoff;
    public delegate void InfectedAirplaneTakeoffEventHandler(Airplane airplane);

    // Use this for initialization
    void Awake ()
    {
        flightNumber = Random.Range(101, 2900);
    }
	
    public void Infect(Virus virus)
    {
        this.virus = virus;
        planeAnimator.SetBool("isInfected", true);
    }

    public void TakeOff()
    {
        if (virus != null && OnInfectedAirplaneTakeoff != null)
        {
            OnInfectedAirplaneTakeoff(this);
        }
        SetPosition(source);
        planeAnimator.SetTrigger("Takeoff");
    }

    public void TakeoffComplete()
    {
        SetPosition(destination);
    }

    public void LandingComplete()
    {
        if (virus != null)
        {
            destination.Infect(virus);
        }
        Destroy(gameObject);
    }

    private void SetPosition(InfectionPoint point)
    {
        transform.localPosition = globe.GetGlobePosition(point.x, point.y) * 1.01f;
        transform.localRotation = Quaternion.LookRotation(transform.localPosition, globe.Earth.transform.up);
        transform.Rotate(new Vector3(0, 90, 0));
    }
}
