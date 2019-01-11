using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VirusMod: MonoBehaviour {
    public string modName;

    public abstract void Apply(Virus virus);
    public abstract void Remove(Virus virus);
}
