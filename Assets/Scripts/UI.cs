using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class UI : MonoBehaviour {
    //editor mode members
    public GameObject uiPanel;
    public DataLoader loader;
    public DataEditor editor;
    public Dropdown countryList;
    public Dropdown regionList;
    public Dropdown healthCareList;
    public GameObject dataPlane;
    public EditMode editMode = EditMode.COUNTRIES;

    //live members
    public string SelectedCountry;
    
    // Use this for initialization
    void Start ()
    {
        SetCountryMode();
        HideUI();
        FillCountries();
        FillRegions();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            if (uiPanel.activeInHierarchy)
            {
                HideUI();
            }
            else
            {
                ShowUI();
            }

            Camera.main.transform.parent.GetComponentInChildren<FirstPersonController>().enabled = !uiPanel.activeInHierarchy;
        }
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

    public void SetCountryMode()
    {
        SetToolbarMode(EditMode.COUNTRIES);
    }

    public void SetRegionMode()
    {
        SetToolbarMode(EditMode.REGIONS);
    }

    public void SetHealthcareMode()
    {
        SetToolbarMode(EditMode.HEALTHCARE);
    }

    public void SetAirportMode()
    {
        SetToolbarMode(EditMode.AIRPORTS);
    }

    public void SetToolbarMode(EditMode editMode)
    {
        this.editMode = editMode;

        GameObject.Find("CountryControls").GetComponent<CanvasGroup>().alpha = editMode == EditMode.COUNTRIES ? 1f : 0f;
        GameObject.Find("RegionControls").GetComponent<CanvasGroup>().alpha = editMode == EditMode.REGIONS ? 1f : 0f;
        GameObject.Find("HealthcareControls").GetComponent<CanvasGroup>().alpha = editMode == EditMode.HEALTHCARE ? 1f : 0f;
        GameObject.Find("AirportControls").GetComponent<CanvasGroup>().alpha = editMode == EditMode.AIRPORTS ? 1f : 0f;

        GameObject.Find("EditModeCountry").GetComponent<Image>().color = editMode == EditMode.COUNTRIES ? Color.cyan : Color.white;
        GameObject.Find("EditModeRegion").GetComponent<Image>().color = editMode == EditMode.REGIONS ? Color.cyan : Color.white;
        GameObject.Find("EditModeHealthcare").GetComponent<Image>().color = editMode == EditMode.HEALTHCARE ? Color.cyan : Color.white;
        GameObject.Find("EditModeAirport").GetComponent<Image>().color = editMode == EditMode.AIRPORTS ? Color.cyan : Color.white;
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

    public void LoadRegionData()
    {
        editor.LoadRegions();
        editor.SaveWorldData("Assets/Resources/WorldDataBackup.json");
    }

    public void LoadHealthcareData()
    {
        editor.LoadHealthcare();
        editor.SaveWorldData("Assets/Resources/WorldDataBackup.json");
    }

    public void LoadAirportData()
    {
        editor.LoadAirports();
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
    
    public void SetRegion()
    {
        editor.SetRegionText(regionList.captionText.text);
    }

    public void ClearRegion()
    {
        editor.SetRegionText("");
    }

    public void SetAirport()
    {
        editor.SetAirport(true);
    }

    public void ClearAirport()
    {
        editor.SetAirport(false);
    }

    public void SetHealthcare()
    {
        editor.SetHealthCare(healthCareList.value + 1);
    }

    public void ClearHealthcare()
    {
        editor.SetHealthCare(0);
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

    private void FillRegions()
    {
        regionList.ClearOptions();

        regionList.options.Add(new Dropdown.OptionData("North America"));
        regionList.options.Add(new Dropdown.OptionData("South America"));
        regionList.options.Add(new Dropdown.OptionData("Europe"));
        regionList.options.Add(new Dropdown.OptionData("Africa"));
        regionList.options.Add(new Dropdown.OptionData("Asia"));
        regionList.options.Add(new Dropdown.OptionData("Russia"));
        regionList.options.Add(new Dropdown.OptionData("Oceania"));
    }

    public void SelectCountryForDeployment(Text CountryText)
    {
        SelectedCountry = CountryText.text;
    }
        
    public void LogMessage(string message)
    {
        Debug.Log(message);
    }
}
