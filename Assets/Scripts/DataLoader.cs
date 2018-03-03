using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

public class DataLoader : MonoBehaviour {    
    public WorldData LoadJSON(string resourceName)
    {
        TextAsset jsonData = Resources.Load<TextAsset>(resourceName);
        string json = jsonData.text;
        return JsonUtility.FromJson<WorldData>(json);
    }
        
    public void CreateCountryData()
    {
        DataPoint[] points = GameObject.Find("DataPlane").GetComponentsInChildren<DataPoint>();
        WorldData world = new WorldData();
        List<CountryData> countryDataList = new List<CountryData>();
        string[] countries = GetAllCountries();

        foreach (string country in countries)
        {
            List<CountryPoint> countryPoints = new List<CountryPoint>();
            CountryData countryData = new CountryData();

            foreach (DataPoint point in points)
            {
                if (point.CountryName == country)
                {
                    CountryPoint countryPoint = new CountryPoint();
                    countryPoint.x = point.x;
                    countryPoint.y = point.y;
                    countryPoint.population = point.population;
                    countryPoint.infection = point.infection;
                    countryPoints.Add(countryPoint);
                }
            }

            countryData.Name = country;
            countryData.Points = countryPoints.ToArray();
            countryDataList.Add(countryData);
        }

        world.Countries = countryDataList.ToArray();

        string worldJson = JsonUtility.ToJson(world);
                
        string filePath = "Resources/WorldData.json";
        File.WriteAllText(filePath, worldJson);
    }

    public string[] GetAllCountries()
    {
        List<string> countryNames = new List<string>();
        WorldData worldCountries = LoadJSON("CountryData");

        foreach (CountryData country in worldCountries.Countries)
        {
            countryNames.Add(country.Name);
        }

        return countryNames.ToArray();
    }

    public void ConvertToCountryData()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("population");
        SeriesArray data = JsonUtility.FromJson<SeriesArray>(jsonData.text);

        CountryData[] countryDataList = new CountryData[data.AllData.Length];
        for (int i = 0; i < data.AllData.Length; i++)
        {
            SeriesData seriesData = data.AllData[i];

            CountryData countryData = new CountryData();
            countryDataList[i] = countryData;
            countryData.Name = "UNKNOWN";
            countryData.Points = new CountryPoint[seriesData.Data.Length/3];

            for (int j = 0, k = 0; j < seriesData.Data.Length; j += 3, k++)
            {
                CountryPoint p = new CountryPoint();
                p.y = Convert.ToInt32(seriesData.Data[j]);
                p.x = Convert.ToInt32(seriesData.Data[j + 1]);
                p.population = seriesData.Data[j + 2];
                p.infection = 0;
                countryData.Points[k] = p;
            }
        }
        
        WorldData worldData = new WorldData();
        worldData.Countries = countryDataList;
        string json = JsonUtility.ToJson(worldData);
    }
}

[System.Serializable]
public class SeriesArray
{
    public SeriesData[] AllData;
}

[System.Serializable]
public class SeriesData
{
    public string Name;
    public float[] Data;
}