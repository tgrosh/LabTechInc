using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distributor : MonoBehaviour {
    public Canvas distributorScreen;
    public Transform containerPlacemat;
    public Virus virus;

	// Use this for initialization
	void Start () {
        distributorScreen.gameObject.SetActive(false);
    }
	
    void OnTriggerEnter(Collider other)
    {
        virus = other.transform.root.GetComponent<Virus>();

        if (virus != null)
        {
            distributorScreen.gameObject.SetActive(true);
            other.transform.root.GetComponent<Rigidbody>().MovePosition(containerPlacemat.position);
            other.transform.root.GetComponent<Rigidbody>().MoveRotation(containerPlacemat.rotation);
            other.transform.root.GetComponent<Rigidbody>().isKinematic = true;
        }        
    }

    void OnTriggerExit(Collider other)
    {
        if (virus != null)
        {
            distributorScreen.gameObject.SetActive(false);
        }
    }
}
