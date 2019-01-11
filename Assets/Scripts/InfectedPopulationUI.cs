using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class InfectedPopulationUI : MonoBehaviour {
    public Text worldPopulation;
    public Text infectedPopulation;
    public Image progress;

	// Update is called once per frame
	void Update () {
        worldPopulation.text = World.instance.totalPopulation.ToString("N0");        
        infectedPopulation.text = World.instance.infectedPopulation.ToString("N0");

        progress.fillAmount = 1f * World.instance.infectedPopulation / World.instance.totalPopulation;
    }
}
