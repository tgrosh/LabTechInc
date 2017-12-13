using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataEditor : MonoBehaviour {
    public DataLoader loader;
    public GameObject DataPointPrefab;

    // Use this for initialization
    void Start () {
        Load("dataPoints");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    private void Load(string resourceName)
    {
        CountryData[] countries = loader.LoadJSON(resourceName).Countries;
        Bounds bounds = GetComponent<MeshFilter>().mesh.bounds;

        foreach (CountryData countryData in countries)
        {
            foreach (CountryPoint countryPoint in countryData.Points)
            {
                GameObject p = Instantiate<GameObject>(DataPointPrefab);
                p.transform.parent = transform;
                p.transform.localPosition = new Vector3(-1* (bounds.size.x / 360 * countryPoint.x), 0, -1 * (bounds.size.z / 180 * countryPoint.y));
                p.transform.rotation = Quaternion.Euler(Vector3.left);
                p.transform.localScale = new Vector3(10, 20, .005f);

                DataPoint pdata = p.GetComponent<DataPoint>();
                pdata.x = countryPoint.x;
                pdata.y = countryPoint.y;
                pdata.population = countryPoint.population;
                pdata.countryName = countryData.Name;
            }
        }
    }
}
