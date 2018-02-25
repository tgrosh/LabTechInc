using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataEditor : MonoBehaviour {
    public DataLoader loader;
    public GameObject DataPointPrefab;
    public Gradient healthCareGradient;
    Bounds bounds;
    DataPoint[] allPoints;
    private Dictionary<string, Color> countryColors = new Dictionary<string, Color>();
    private Dictionary<string, Color> regionColors = new Dictionary<string, Color>();
    CountryData[] countries;

    // Use this for initialization
    void Start () {
        bounds = GetComponent<MeshFilter>().mesh.bounds;
	}
	
    public void LoadCountries()
    {
        Load("worldData", EditMode.COUNTRIES);
    }

    public void LoadRegions()
    {
        Load("worldData", EditMode.REGIONS);
    }

    public void LoadHealthcare()
    {
        Load("worldData", EditMode.HEALTHCARE);
    }

    public void LoadAirports()
    {
        Load("worldData", EditMode.AIRPORTS);
    }

    private void Load(string worldDataResourceName, EditMode editMode)
    {
        CreateEmptyGrid();
        LoadWorld(worldDataResourceName);
        OverlayWorldData(worldDataResourceName, editMode);
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
    
    private void OverlayWorldData(string resourceName, EditMode editMode)
    {        
        foreach (CountryData countryData in countries)
        {
            //temporary
            if (countryData.RegionName == null)
            {
                countryData.RegionName = "";
            }

            foreach (CountryPoint countryPoint in countryData.Points)
            {
                DataPoint pdata = FindDataPoint(countryPoint.x, countryPoint.y);
                if (pdata != null)
                {
                    switch (editMode)
                    {
                        case EditMode.COUNTRIES:
                            pdata.color = getCountryColor(countryData.Name);
                            break;
                        case EditMode.HEALTHCARE:
                            pdata.color = getHealthCareColor(countryPoint.healthCare);
                            break;
                        case EditMode.AIRPORTS:
                            pdata.color = getAirportColor(countryPoint.isAirport);
                            break;
                        case EditMode.REGIONS:
                            pdata.color = getRegionColor(countryData.RegionName);
                            break;
                        default:
                            break;
                    }
                    pdata.population = countryPoint.population;
                    pdata.Populated = true;
                    pdata.CountryName = countryData.Name;
                    pdata.RegionName = countryData.RegionName;
                    pdata.HealthCare = countryPoint.healthCare;
                    pdata.IsAirport = countryPoint.isAirport;
                }
            }
        }
    }

    private void LoadWorld(string resourceName)
    {
        countries = loader.LoadJSON(resourceName).Countries;
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

    public void SetRegionText(string regionText)
    {
        DataPoint[] points = GetComponentsInChildren<DataPoint>();
        foreach (DataPoint point in points)
        {
            if (point.Selected)
            {
                point.RegionName = regionText;
                point.color = getRegionColor(regionText);
                point.Selected = false;
            }
        }
    }

    public void SetHealthCare(float healthCare)
    {
        DataPoint[] points = GetComponentsInChildren<DataPoint>();
        foreach (DataPoint point in points)
        {
            if (point.Selected)
            {
                point.color = getHealthCareColor(healthCare/10f);
                point.HealthCare = healthCare/10f;
                point.Selected = false;
            }
        }        
    }

    public void SetAirport(bool isAirport)
    {
        DataPoint[] points = GetComponentsInChildren<DataPoint>();
        foreach (DataPoint point in points)
        {
            if (point.Selected)
            {
                point.color = getAirportColor(isAirport);
                point.IsAirport = isAirport;
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

    public Color getRegionColor(string regionName)
    {
        Color color = Color.clear;

        if (string.IsNullOrEmpty(regionName))
        {
            return Color.white;
        }

        if (regionColors.ContainsKey(regionName))
        {
            color = regionColors[regionName];
        }
        else
        {
            color = Random.ColorHSV(.05f, .95f, .5f, 1f, .5f, 1f, 1, 1);
            regionColors.Add(regionName, color);
        }

        return color;
    }

    public Color getHealthCareColor(float healthCare)
    {
        return healthCareGradient.Evaluate(healthCare);
    }

    public Color getAirportColor(bool isAirport)
    {
        return isAirport ? Color.red : Color.white;
    }

    public void SaveWorldData(string resourceName)
    {
        WorldData world = new WorldData();
        List<CountryData> countries = new List<CountryData>();
        var pointGroups = new List<DataPoint>(allPoints).GroupBy(p => p.CountryName).ToList();
        
        foreach (var group in pointGroups)
        {
            CountryData country = new CountryData();
            country.Name = group.Key;

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
                    cp.healthCare = point.HealthCare;
                    cp.isAirport = point.IsAirport;
                    countryPoints.Add(cp);

                    if (string.IsNullOrEmpty(country.RegionName) && !string.IsNullOrEmpty(point.RegionName))
                    {
                        country.RegionName = point.RegionName;
                    }
                }
            }
            country.Points = countryPoints.ToArray();

            countries.Add(country);
        }

        world.Countries = countries.ToArray();

        File.WriteAllText(resourceName, JsonUtility.ToJson(world));
    }
}

public enum EditMode
{
    COUNTRIES,
    HEALTHCARE,
    AIRPORTS,
    REGIONS
}
