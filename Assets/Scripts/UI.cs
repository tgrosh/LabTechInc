using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public GameObject uiPanel;
    public DataLoader loader;
    public DataEditor editor;
    public Dropdown countryList;
    public GameObject dataPlane;
    

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

    public void SaveWorldData()
    {
        editor.SaveWorldData("Assets/Resources/WorldData.json");
    }

    public void LoadCountryData()
    {
        editor.LoadCountries();
        editor.SaveWorldData("Assets/Resources/WorldDataBackup.json");
    }

    public void SetCountry()
    {
        editor.SetCountryText(countryList.captionText.text);
    }

    public void ClearCountry()
    {
        editor.SetCountryText("");
    }
        
    private void FillCountries()
    {
        List<string> countries = new List<string>(loader.GetAllCountries());
        countryList.ClearOptions();
        countries.Sort();

        foreach (string countryName in countries)
        {
            countryList.options.Add(new Dropdown.OptionData(countryName));
        }        
    }

    
}
