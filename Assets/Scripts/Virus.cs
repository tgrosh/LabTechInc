using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus: MonoBehaviour {
    public float infectionRate;
    public List<VirusMod> mods;

    public void ApplyMod(VirusMod mod)
    {
        if (!mods.Contains(mod))
        {
            mods.Add(mod);
            mod.Apply(this);
        }        
    }
}
