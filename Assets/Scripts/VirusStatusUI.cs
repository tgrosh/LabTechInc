using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirusStatusUI : MonoBehaviour {
    public Image[] statusImages;

    public VirusStatus currentStatus = VirusStatus.Development;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (World.instance.currentVirus.status != currentStatus)
        {
            currentStatus = World.instance.currentVirus.status;

            foreach (Image img in statusImages)
            {
                if (img.gameObject.name == currentStatus.ToString())
                {
                    img.color = Color.cyan;
                } else
                {
                    img.color = Color.white;
                }
            }
        }
	}
}
