using UnityEngine;

public class InfectionPoint: CountryPoint
{
    public string countryName;
    public int vertIndex;
    public Virus virus;

    public static event InfectionUpdatedEventHandler OnInfectionPointUpdated;
    public delegate void InfectionUpdatedEventHandler(InfectionPoint point);

    public void Infect(Virus virus)
    {
        Debug.Log(countryName + " is now infected");
        this.virus = virus;
        this.virus.hostPoint = this;
        this.virus.active = true;
        Infection = .01f;
    }

    public float Infection {
        get
        {
            return infection;
        }
        set
        {
            infection = value;

            if (OnInfectionPointUpdated != null)
            {
                //Debug.Log("Infection Points updating for " + countryName);
                OnInfectionPointUpdated(this);
            }
        }
    }
}
