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
    public Text hotbarTooltip;
    public Text helpText;
    public EditMode editMode = EditMode.COUNTRIES;
        
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

            GameObject.Find("FPSController").GetComponent<FirstPersonController>().enabled = !uiPanel.activeInHierarchy;
        } else if (Input.GetKeyUp(KeyCode.F2))
        {
            GameObject.Find("OVRPlayerController").gameObject.SetActive(false);
            GameObject.Find("FPSController").GetComponent<FirstPersonController>().enabled = true;            
        }
    }

    public void SetTooltip(string text)
    {
        if (text == "")
        {
            hotbarTooltip.text = "Select an Edit Mode";
        } else
        {
            hotbarTooltip.text = text;
        }
    }

    public void SetHelpText(string text)
    {
        helpText.text = text;
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
        SetHelpText("Country Mode\nSelect Country Name from dropdown\nLeft Click on existing nodes. Click Assign\nHold Alt key to prevent overwriting existing\nHold Left Ctrl to add new nodes");
    }

    public void SetRegionMode()
    {
        SetToolbarMode(EditMode.REGIONS);
        SetHelpText("Region Mode\nSelect Region Name from dropdown\nLeft Click on existing nodes. Click Assign\nHold Alt key to constrain to country borders\nHold Left Ctrl to add new nodes");
    }

    public void SetHealthcareMode()
    {
        SetToolbarMode(EditMode.HEALTHCARE);
        SetHelpText("Healthcare Mode\nSelect Region Name from dropdown\nLeft Click on existing nodes. Click Assign\nHold Alt key to prevent overwriting existing\nHold Left Ctrl to add new nodes");
    }

    public void SetAirportMode()
    {
        SetToolbarMode(EditMode.AIRPORTS);
        SetHelpText("Airport Mode\nLeft Click on existing nodes. Click Assign\nHold Left Ctrl to add new nodes");
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
            
    public void LogMessage(string message)
    {
        Debug.Log(message);
    }
}
