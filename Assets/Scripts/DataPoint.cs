using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPoint : MonoBehaviour {
    public int x;
    public int y;
    public float population;
    public float infection;
    public Color color;

    private Color originalColor = Color.white;
    private Color currentColor = Color.clear;
    private bool populated;
    private string countryName;
    private string regionName;
    private bool selected;
    private float healthCare;
    private bool isAirport;

    // Use this for initialization
    void Start () {        
        
    }

    public bool Selected {
        get { return selected; }
        set {
            selected = value;
            setColor();
        }
    }

    public bool Populated {
        get
        {
            return populated;
        }
        set
        {
            populated = value;
            GetComponent<MeshRenderer>().enabled = value;
            setColor();
        }
    }

    public string CountryName {
        get { return countryName; }
        set
        {
            countryName = value;
            setColor();
        }
    }

    public string RegionName
    {
        get { return regionName; }
        set
        {
            regionName = value;
            setColor();
        }
    }

    public float HealthCare {
        get
        {
            return healthCare;
        }
        set
        {
            healthCare = value > 1 ? 1 : value;
            setColor();
        }
    }

    public bool IsAirport {
        get { return isAirport; }
        set
        {
            isAirport = value;
            setColor();
        }
    }

    private void setColor()
    {
        if (populated && selected && currentColor != Color.cyan)
        {
            currentColor = gameObject.GetComponent<Renderer>().material.color = Color.cyan;
        }
        else if (populated && !selected && currentColor != color)
        {
            currentColor = gameObject.GetComponent<Renderer>().material.color = color;
        }
        else if (populated && !selected && currentColor != originalColor &&
            string.IsNullOrEmpty(regionName) && string.IsNullOrEmpty(countryName))
        {
            currentColor = gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
    }
    
}
