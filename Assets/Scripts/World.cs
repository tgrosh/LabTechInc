using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class World : MonoBehaviour {
    public DataLoader loader;
    public DataGlobe globe;
    public InfectionPoint[][] infectionPoints = new InfectionPoint[180][];
    public List<InfectionPoint> airports = new List<InfectionPoint>();
    public float drawInterval = 1.0f;
    public InfectionPoint infectionPointPrefab;
    Virus virus = new Virus(1.5f);
    
    void Awake()
    {
    }

	// Use this for initialization
	void Start ()
    {
        for (int y = 0; y < 180; y++)
        {
            infectionPoints[y] = new InfectionPoint[360];
        }

        InfectionPoint.OnInfectionPointUpdated += InfectionPoint_OnInfectionPointUpdated;
        LoadInfectionPoints();

        TestUpdateInfectionPoints();
    }
	
	// Update is called once per frame
	void Update () {
    }

    void TestUpdateInfectionPoints()
    {
        int numPointsToTest = 10;
        
        for (int p=0; p<numPointsToTest; p++)
        {
            int randomX = UnityEngine.Random.Range(0, 360);
            int randomY = UnityEngine.Random.Range(0, 180);
            InfectionPoint point = infectionPoints[randomY][randomX];
            if (point != null)
            {
                point.Infect(virus);
            }
        }        
    }
    
    public List<int> GetAdjacentInfectionPointIndexes(int x, int y)
    {
        List<int> result = new List<int>();

        for (int ix = x - 1; ix <= x + 1; ix++)
        {
            for (int iy = y - 1; iy <= y + 1; iy++)
            {
                if (ix >= 0 && iy >= 0 && ix < 360 && iy < 180 && !(x == ix && y == iy))
                {
                    result.Add(iy*360 + ix);
                }
            }
        }

        return result;
    }

    private void LoadInfectionPoints()
    {
        CountryData[] countries = loader.LoadJSON("WorldData").Countries;
        foreach (CountryData countryData in countries)
        {
            foreach (CountryPoint countryPoint in countryData.Points)
            {
                //InfectionPoint pt = new InfectionPoint();
                InfectionPoint pt = Instantiate(infectionPointPrefab, gameObject.transform);
                infectionPoints[countryPoint.y + 90][countryPoint.x + 180] = pt;

                pt.x = countryPoint.x;
                pt.y = countryPoint.y;
                pt.infection = countryPoint.infection;
                pt.population = countryPoint.population;
                pt.countryName = countryData.Name;
                pt.world = this;
                pt.healthCare = countryPoint.healthCare - 0.1f;
                pt.adjacentInfectionPointIndexes = GetAdjacentInfectionPointIndexes(countryPoint.x + 180, countryPoint.y + 90);
                pt.temperatureFactor = (Mathf.Abs(Mathf.Abs(pt.y) - 45f) / 45f) * .25f; //should be range from 0 to .25
                pt.isAirport = countryPoint.isAirport;
                if (pt.isAirport)
                {
                    airports.Add(pt);
                }
            }
        }
        globe.CreateWorldMeshes(this);
    }

    public InfectionPoint GetRandomAirport()
    {
        return airports[UnityEngine.Random.Range(0, airports.Count)];
    }

    private void InfectionPoint_OnInfectionPointUpdated(InfectionPoint point)
    {
        if (globe.isReady)
        {
            globe.UpdateInfectionPointColor(point);
        }
    }
}
