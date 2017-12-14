using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPoint : MonoBehaviour {
    public int x;
    public int y;
    public float population;
    public float infection; 
    private bool selected;
    private string countryName;
    public Color color;
    private Color originalColor = Color.white;
    private Color currentColor = Color.clear;
    private bool populated;

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

    private void setColor()
    {
        if (populated && selected && currentColor != Color.cyan)
        {
            //originalColor = gameObject.GetComponent<Renderer>().material.color;
            //Debug.Log("Setting originalColor to " + originalColor);
            currentColor = gameObject.GetComponent<Renderer>().material.color = Color.cyan;
            //Debug.Log("Setting color to cyan");
        }
        else if (populated && !selected && countryName != "" && countryName != "UNKNOWN" && currentColor != color)
        {
            currentColor = gameObject.GetComponent<Renderer>().material.color = color;
            //Debug.Log("Setting color to country color " + color);
        }
        else if (populated && !selected && string.IsNullOrEmpty(countryName) && currentColor != originalColor)
        {
            //Debug.Log("Setting color to original color " + originalColor);
            currentColor = gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
    }
    
}
