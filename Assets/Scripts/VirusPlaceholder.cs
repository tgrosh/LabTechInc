using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusPlaceholder : MonoBehaviour {
    public Virus virusPrefab;
    public Virus currentVirus;
    public Placemat placemat;

	// Use this for initialization
	void Start () {
        placemat = GetComponent<Placemat>();
        placemat.OnPlacematEnter += Placemat_OnPlacematEnter;
        placemat.OnPlacematExit += Placemat_OnPlacematExit;

        GetComponentInChildren<MeshRenderer>().enabled = false;
        CreateVirus();
	}

    private void Placemat_OnPlacematExit(GameObject placedObject)
    {
        currentVirus = null;
    }

    private void Placemat_OnPlacematEnter(GameObject placedObject)
    {
        currentVirus = placedObject.transform.root.GetComponent<Virus>();
        currentVirus.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void CreateVirus()
    {
        Instantiate(virusPrefab, transform.position, transform.rotation);
    }
}
