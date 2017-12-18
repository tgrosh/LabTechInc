using System.Collections;
using UnityEngine;

public class InfectionPoint: MonoBehaviour
{
    public int x;
    public int y;
    public float population;
    public float infection;
    public string countryName;
    public int vertIndex;
    public Virus virus;
    public float updateInterval = 1.0f;

    float currentTime = 0f;
    float prevInfection = 0f;

    public static event InfectionUpdatedEventHandler OnInfectionPointUpdated;
    public delegate void InfectionUpdatedEventHandler(InfectionPoint point);

    public void Infect(Virus virus)
    {
        //Debug.Log(countryName + " is now infected");
        this.virus = virus;
        Infection = .001f;
    }

    public float Infection {
        get
        {
            return infection;
        }
        set
        {
            prevInfection = infection;
            infection = value > 1 ? 1 : value;

            if (prevInfection != infection && OnInfectionPointUpdated != null)
            {
                OnInfectionPointUpdated(this);
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (virus != null && currentTime >= updateInterval)
        {
            Infection *= virus.infectionRate;
            currentTime = 0;
        }
        currentTime += Time.deltaTime;
    }
}
