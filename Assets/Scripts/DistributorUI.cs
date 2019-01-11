using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistributorUI : MonoBehaviour {
    public World theWorld;
    public Distributor distributor;
    public string SelectedRegion;
    public VirusPlaceholder[] virusPlaceholders;
    public GameObject instructionPanel;
    public GameObject deploymentPanel;

    public void SelectCountryForDeployment(Text CountryText)
    {
        SelectedRegion = CountryText.text;
    }

    public void EnableDeployment(bool enable)
    {
        instructionPanel.SetActive(!enable);
        deploymentPanel.SetActive(enable);
    }

    public void DeployVirus()
    {
        if (distributor.virus != null)
        {
            theWorld.DeployVirus(distributor.virus, SelectedRegion);
            distributor.virus.gameObject.transform.position = new Vector3(10000, 10000, 0);
        }

        foreach (VirusPlaceholder placeholder in virusPlaceholders)
        {
            if (placeholder.currentVirus == null)
            {
                placeholder.CreateVirus();
                break;
            }
        }
    }
}
