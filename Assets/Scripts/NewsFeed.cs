using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsFeed : MonoBehaviour {
    public List<string> newsFeed;
    private List<string> infectedCountries = new List<string>();

	// Use this for initialization
	void Start () {
        InfectionPoint.OnInfectionPointUpdated += InfectionPoint_OnInfectionPointUpdated;
        Airplane.OnInfectedAirplaneTakeoff += Airplane_OnInfectedAirplaneTakeoff;
	}

    private void Airplane_OnInfectedAirplaneTakeoff(Airplane airplane)
    {
        newsFeed.Add("A passenger aboard flight " + airplane.flightNumber + " from " + airplane.source.countryName + " to " + airplane.destination.countryName + " is infected");
    }

    private void InfectionPoint_OnInfectionPointUpdated(InfectionPoint point)
    {
        if (infectedCountries.Count == 0)
        {
            newsFeed.Add("A new virus has been introduced in " + point.countryName);
        }
        if (!infectedCountries.Contains(point.countryName))
        {
            newsFeed.Add("The virus has spread to " + point.countryName);
            infectedCountries.Add(point.countryName);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
