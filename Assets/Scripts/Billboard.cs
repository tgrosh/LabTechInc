﻿using UnityEngine;

public class Billboard : MonoBehaviour { 
	void Update() { 
		transform.LookAt(Camera.main.transform.position, gameObject .transform.up); 
		//transform.rotation = Camera.main.transform.rotation;
	} 
}