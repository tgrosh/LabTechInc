using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistributorUI : MonoBehaviour {
    public World theWorld;
    public Distributor distributor;
    public string SelectedRegion;

    public void SelectCountryForDeployment(Text CountryText)
    {
        SelectedRegion = CountryText.text;
    }

    public void DeployVirus()
    {
        if (distributor.virus != null)
        {
            theWorld.DeployVirus(distributor.virus, SelectedRegion);
        }
    }
}
