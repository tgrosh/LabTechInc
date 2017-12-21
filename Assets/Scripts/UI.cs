using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class UI : MonoBehaviour {
    public GameObject uiPanel;
    public DataLoader loader;
    public DataEditor editor;
    public Dropdown countryList;
    public Dropdown healthCareList;
    public GameObject dataPlane;
    public EditMode editMode = EditMode.COUNTRIES;
    
    // Use this for initialization
    void Start ()
    {
        SetCountryMode();
        HideUI();
        FillCountries();
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
        editMode = EditMode.COUNTRIES;
        GameObject.Find("CountryControls").GetComponent<CanvasGroup>().alpha = 1f;
        GameObject.Find("HealthcareControls").GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("EditModeCountry").GetComponent<Image>().color = Color.cyan;
        GameObject.Find("EditModeHealthcare").GetComponent<Image>().color = Color.white;
    }

    public void SetHealthcareMode()
    {
        editMode = EditMode.HEALTHCARE;
        GameObject.Find("CountryControls").GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("HealthcareControls").GetComponent<CanvasGroup>().alpha = 1f;
        GameObject.Find("EditModeCountry").GetComponent<Image>().color = Color.white;
        GameObject.Find("EditModeHealthcare").GetComponent<Image>().color = Color.cyan;
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

    public void LoadHealthcareData()
    {
        editor.LoadHealthcare();
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

    
}
