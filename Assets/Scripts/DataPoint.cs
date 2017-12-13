using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPoint : MonoBehaviour {
    public int x;
    public int y;
    public float population;
    public float infection;
    public bool selected;
    public string countryName;
    public Color color;

    private int nameValue;
    private Color originalColor = Color.clear;
    private Color currentColor = Color.clear;
    
    // Use this for initialization
    void Start () {        
        
    }
	
	// Update is called once per frame
	void Update () {
		if (selected && currentColor != Color.cyan)
        {
            originalColor = gameObject.GetComponent<Renderer>().material.color;
            currentColor = gameObject.GetComponent<Renderer>().material.color = Color.cyan;
        } else if (!selected && countryName != "" && countryName != "UNKNOWN" && currentColor != color)
        {
            currentColor = gameObject.GetComponent<Renderer>().material.color = color;
        }
        else if (!selected && originalColor != Color.clear)
        {
            gameObject.GetComponent<Renderer>().material.color = originalColor;
            originalColor = currentColor = Color.clear;
        }
    }
}
