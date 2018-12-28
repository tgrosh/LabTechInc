using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicroscopeUI : MonoBehaviour {
    public Virus virus;
    public Button applyButton;
    public Toggle[] toggles;

    private Toggle selectedToggle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        bool interactable = false;

        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn)
            {
                interactable = true;
                break;
            }
        }

        applyButton.interactable = interactable;
    }

    public void OnSelectToggle(Toggle toggle)
    {
        if (toggle.isOn)
        {
            selectedToggle = toggle;
        } else
        {
            selectedToggle = null;
        }
    }

    public void ApplyVirusMod()
    {
        VirusMod mod = selectedToggle.GetComponent<VirusMod>();

        if (mod != null)
        {
            virus.ApplyMod(mod);
        }
    }
}
