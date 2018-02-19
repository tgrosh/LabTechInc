using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIToucher : MonoBehaviour {
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
            if (onDown) ExecuteEvents.Execute(touchable.gameObject, pointer, ExecuteEvents.pointerDownHandler);
            if (onClick) ExecuteEvents.Execute(touchable.gameObject, pointer, ExecuteEvents.pointerClickHandler);
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "IndexFinger")
        {
            var pointer = new PointerEventData(EventSystem.current);
            if (onUp) ExecuteEvents.Execute(touchable.gameObject, pointer, ExecuteEvents.pointerUpHandler);
            if (onEnter) ExecuteEvents.Execute(touchable.gameObject, pointer, ExecuteEvents.pointerExitHandler);
        }
    }
}
