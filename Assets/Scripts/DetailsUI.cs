using UnityEngine;
using UnityEngine.UI;

public class DetailsUI : MonoBehaviour {
    public Text nameUI;
    public Text rateUI;
    public Text modsUI;
    public Virus virus;

    // Use this for initialization
    void Start () {
        nameUI.text = "";
        rateUI.text = "";
        modsUI.text = "";
    }
	
	// Update is called once per frame
	void Update () {
		if (World.instance.currentVirus != null)
        {
            nameUI.text = World.instance.currentVirus.virusName;
            rateUI.text = World.instance.currentVirus.infectionRate.ToString();
            modsUI.text = "";
            foreach (VirusMod mod in World.instance.currentVirus.mods)
            {
                modsUI.text += "- " + mod.modName + "\n"; //bad coding
            }
        }
	}
}
