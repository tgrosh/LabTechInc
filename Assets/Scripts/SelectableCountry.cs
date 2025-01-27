﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableCountry : MonoBehaviour
{
    public GameObject selection;
    private DistributorUI ui;
    private Text text;

    // Use this for initialization
    void Start () {
        ui = GameObject.Find("DistributorCanvas").GetComponent<DistributorUI>();
        text = transform.GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        selection.SetActive(ui.SelectedRegion == text.text);
	}
}
