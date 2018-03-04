using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroscopeDoorUI : MonoBehaviour {
    public Animator doorAnimator;
    
    public void Open()
    {
        doorAnimator.SetBool("isOpen", true);
    }

    public void Close()
    {
        doorAnimator.SetBool("isOpen", false);
    }
}
