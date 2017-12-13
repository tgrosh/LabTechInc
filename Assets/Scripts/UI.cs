using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public GameObject uiPanel;
    public DataLoader loader;
    public Dropdown countryList;
    public GameObject dataPlane;
    
    private Dictionary<string, Color> countryColors = new Dictionary<string, Color>();

    // Use this for initialization
    void Start () {
        HideUI();
        FillCountries();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowUI()
    {
        uiPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideUI()
    {
        uiPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetCountry()
    {
        Color newColor = Random.ColorHSV(0, 1, 0, 1, 0, 1, 1, 1);
        DataPoint[] points = dataPlane.GetComponentsInChildren<DataPoint>();
        foreach (DataPoint point in points)
        {
            if (point.selected)
            {
                point.countryName = countryList.captionText.text;
                point.color = getCountryColor(point.countryName);
                point.selected = false;
            }
        }
    }

    public Color getCountryColor(string countryName)
    {
        Color color = Color.clear;

        if (countryColors.ContainsKey(countryName))
        {
            color = countryColors[countryName];
        } else
        {
            color = Random.ColorHSV(.05f, .95f, .05f, .95f, .05f, .95f, 1, 1);
            countryColors.Add(countryName, color);
        }

        return color;
    }

    private void FillCountries()
    {
        string[] countries = loader.GetAllCountries();
        countryList.ClearOptions();

        foreach (string countryName in countries)
        {
            countryList.options.Add(new Dropdown.OptionData(countryName));
        }        
    }

    
}
