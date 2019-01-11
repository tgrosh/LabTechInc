using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirusStatusUI : MonoBehaviour {
    public Virus virus;
    public Image[] statusImages;

    public VirusStatus currentStatus = VirusStatus.Development;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (virus.status != currentStatus)
        {
            foreach (Image img in statusImages)
            {
                if (img.gameObject.name == virus.status.ToString())
                {
                    img.color = Color.cyan;
                } else
                {
                    img.color = Color.white;
                }
            }
            currentStatus = virus.status;
        }
	}
}
