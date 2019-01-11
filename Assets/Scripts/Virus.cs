using System.Collections.Generic;
using UnityEngine;

public class Virus: MonoBehaviour {
    public string virusName;
    public VirusStatus status;
    public float infectionRate;
    public List<VirusMod> mods;

    private void Reset()
    {
        status = VirusStatus.Development;
    }

    public void ApplyMod(VirusMod mod)
    {
        if (!mods.Contains(mod))
        {
            mods.Add(mod);
            mod.Apply(this);
        }        
    }
}

public enum VirusStatus
{
    Development,
    Live,
    Discovered,
    Identified,
    Vaccinated,
    Eradicated
}