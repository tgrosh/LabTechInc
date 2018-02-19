using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoverer : MonoBehaviour {
    public RectTransform touchable;
    public float triggerHeight;
    public bool onEnter;
    public bool onExit;
    public bool onDown;
    public bool onUp;
    public bool onClick;
    
    void Start()
    {
        BoxCollider coll = gameObject.AddComponent<BoxCollider>();
        coll.size = new Vector3(touchable.rect.width, touchable.rect.height, triggerHeight);
        coll.isTrigger = true;
    }    

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "IndexFinger")
        {
            var pointer = new PointerEventData(EventSystem.current);
            if (onEnter) ExecuteEvents.Execute(touchable.gameObject, pointer, ExecuteEvents.pointerEnterHandler);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "IndexFinger")
        {
            var pointer = new PointerEventData(EventSystem.current);
            if (onExit) ExecuteEvents.Execute(touchable.gameObject, pointer, ExecuteEvents.pointerExitHandler);
        }
    }
}
