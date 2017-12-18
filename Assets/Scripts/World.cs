using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class World : MonoBehaviour {
    public static World instance;
    public DataLoader loader;
    public DataGlobe globe;
    public InfectionPoint[][] infectionPoints = new InfectionPoint[180][];
    public float drawInterval = 1.0f;
    public InfectionPoint infectionPointPrefab;
    Virus virus = new Virus(1.5f);

    private float currentDrawTime = 1.0f;

    void Awake()
    {
        instance = this;
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

    public List<InfectionPoint> GetAdjacentInfectionPoints(InfectionPoint infectionPoint)
    {
        int x = infectionPoint.x + 180;
        int y = infectionPoint.y + 90;
        List<InfectionPoint> result = new List<InfectionPoint>();

        for (int ix=x-1; ix<=x+1; ix++)
        {
            for (int iy=y-1; iy<=y+1; iy++)
            {
                if (ix>=0 && iy>=0 && ix<360 && iy<180 && !(x==ix && y==iy))
                {
                    InfectionPoint point = infectionPoints[iy][ix];
                    if (point != null)
                    {
                        result.Add(point);
                    }
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
            }
        }
        globe.CreateWorldMeshes(this);
    }

    private void InfectionPoint_OnInfectionPointUpdated(InfectionPoint point)
    {
        if (globe.isReady)
        {
            globe.UpdateInfectionPointColor(point);
        }
    }
}
