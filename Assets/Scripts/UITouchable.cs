using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITouchable : MonoBehaviour {
    public bool hoverable;

    // Use this for initialization
    void Start () {
        GameObject objToucher = Instantiate<GameObject>(new GameObject("Toucher"), transform);
        UIToucher toucher = objToucher.AddComponent<UIToucher>();
        toucher.gameObject.transform.parent = transform;
        toucher.touchable = GetComponent<RectTransform>();
        toucher.triggerHeight = 5;
        toucher.onClick = toucher.onDown = toucher.onEnter = toucher.onExit = toucher.onUp = true;

        if (hoverable)
        {
            GameObject objHoverer = Instantiate<GameObject>(new GameObject("Hoverer"), transform);
            UIToucher hoverer = objHoverer.AddComponent<UIToucher>();
            hoverer.gameObject.transform.parent = transform;
            hoverer.touchable = GetComponent<RectTransform>();
            hoverer.triggerHeight = 60;
            hoverer.onEnter = hoverer.onExit = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
