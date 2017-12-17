using UnityEngine;

public class InfectionPoint: CountryPoint
{
    public string countryName;
    public int vertIndex;

    public static event InfectionUpdatedEventHandler OnInfectionPointUpdated;
    public delegate void InfectionUpdatedEventHandler(InfectionPoint point);

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
