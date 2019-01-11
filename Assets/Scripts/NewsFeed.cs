using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class NewsFeed : MonoBehaviour {
    public List<string> newsFeed;
    public Text uiNewsFeed;
    public ScrollRect uiNewsFeedScrollRect;
    private List<string> infectedCountries = new List<string>();

	// Use this for initialization
	void Start () {
        InfectionPoint.OnInfectionPointUpdated += InfectionPoint_OnInfectionPointUpdated;
        Airplane.OnInfectedAirplaneTakeoff += Airplane_OnInfectedAirplaneTakeoff;
	}

    private void Airplane_OnInfectedAirplaneTakeoff(Airplane airplane)
    {
        AddNewsFeed("A passenger aboard flight " + airplane.flightNumber + " from " + airplane.source.countryName + " to " + airplane.destination.countryName + " is infected");        
    }

    private void InfectionPoint_OnInfectionPointUpdated(InfectionPoint point)
    {
        if (infectedCountries.Count == 0)
        {
            AddNewsFeed("A new virus has been introduced in " + point.countryName);
        }
        if (!infectedCountries.Contains(point.countryName))
        {
            AddNewsFeed("The virus has spread to " + point.countryName);
            infectedCountries.Add(point.countryName);
        }
    }

    private void AddNewsFeed(string message)
    {
        newsFeed.Add(message);
        UpdateNewsFeedUI();
    }

    private void UpdateNewsFeedUI()
    {
        StringBuilder sb = new StringBuilder();

        foreach (string s in newsFeed)
        {
            sb.Append("[Jan 01, 2034] ").Append(s).Append("\n");
        }

        uiNewsFeed.text = sb.ToString();
        uiNewsFeedScrollRect.verticalNormalizedPosition = 0f;
    }
}
