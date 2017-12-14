using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataEditor : MonoBehaviour {
    public DataLoader loader;
    public GameObject DataPointPrefab;
    Bounds bounds;
    DataPoint[] allPoints;
    private Dictionary<string, Color> countryColors = new Dictionary<string, Color>();

    // Use this for initialization
    void Start () {
        bounds = GetComponent<MeshFilter>().mesh.bounds;
        Load("dataPoints", "worldData");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    private void Load(string populationResourceName, string worldDataResourceName)
    {
        CreateEmptyGrid();
        //OverlayPopulationData(populationResourceName);
        OverlayWorldData(worldDataResourceName);
    }

    private DataPoint FindDataPoint(int x, int y)
    {
        foreach (DataPoint point in allPoints)
        {
            if (point.x == x && point.y == y)
            {
                return point;
            }
        }

        return null;
    }
    
    private void CreateEmptyGrid()
    {
        for (int x=-180; x<180; x++)
        {
            for (int y=-90; y<90; y++)
            {
                GameObject p = Instantiate<GameObject>(DataPointPrefab);
                p.transform.parent = transform;
                p.transform.localPosition = new Vector3(-1 * (bounds.size.x / 360 * x), 0, -1 * (bounds.size.z / 180 * y));

                DataPoint pdata = p.GetComponent<DataPoint>();
                pdata.x = x;
                pdata.y = y;
                pdata.population = 0;
                pdata.CountryName = "";
            }
        }

        allPoints = GetComponentsInChildren<DataPoint>();
    }

    private void OverlayPopulationData(string resourceName)
    {
        CountryData[] countries = loader.LoadJSON(resourceName).Countries;
        foreach (CountryData countryData in countries)
        {
            foreach (CountryPoint countryPoint in countryData.Points)
            {
                DataPoint pdata = FindDataPoint(countryPoint.x, countryPoint.y);
                if (pdata != null)
                {
                    pdata.population = countryPoint.population;
                    pdata.Populated = true;
                }
            }
        }
    }

    private void OverlayWorldData(string resourceName)
    {
        CountryData[] countries = loader.LoadJSON(resourceName).Countries;
        foreach (CountryData countryData in countries)
        {
            foreach (CountryPoint countryPoint in countryData.Points)
            {
                DataPoint pdata = FindDataPoint(countryPoint.x, countryPoint.y);
                if (pdata != null)
                {
                    pdata.population = countryPoint.population;
                    pdata.Populated = true;
                    pdata.color = getCountryColor(countryData.Name);
                    pdata.CountryName = countryData.Name;
                }
            }
        }
    }

    public void SetCountryText(string countryText)
    {
        DataPoint[] points = GetComponentsInChildren<DataPoint>();
        foreach (DataPoint point in points)
        {
            if (point.Selected)
            {
                point.CountryName = countryText;
                point.color = getCountryColor(countryText);
                point.Selected = false;
            }
        }
    }

    public Color getCountryColor(string countryName)
    {
        Color color = Color.clear;

        if (string.IsNullOrEmpty(countryName))
        {
            color = Color.white;
        }

        if (countryColors.ContainsKey(countryName))
        {
            color = countryColors[countryName];
        }
        else
        {
            color = Random.ColorHSV(.05f, .95f, .5f, 1f, .5f, 1f, 1, 1);
            countryColors.Add(countryName, color);
        }

        return color;
    }

    public void SaveWorldData()
    {
        WorldData world = new WorldData();
        List<CountryData> countries = new List<CountryData>();
        var pointGroups = new List<DataPoint>(allPoints).GroupBy(p => p.CountryName).ToList();
        
        foreach (var group in pointGroups)
        {
            List<CountryPoint> countryPoints = new List<CountryPoint>();
            foreach (DataPoint point in group)
            {
                if (point.Populated)
                {
                    CountryPoint cp = new CountryPoint();
                    cp.x = point.x;
                    cp.y = point.y;
                    cp.population = point.population;
                    cp.infection = point.infection;
                    countryPoints.Add(cp);
                }
            }

            CountryData country = new CountryData();
            country.Name = group.Key;
            country.Points = countryPoints.ToArray();

            countries.Add(country);
        }

        world.Countries = countries.ToArray();

        File.WriteAllText("Assets/Resources/WorldData.json", JsonUtility.ToJson(world));
    }
}
