using System.Collections;
using System.Collections.Generic;
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
    public float adjacentTravelChance = .1f; //.001
    public World world;

    List<InfectionPoint> adjacents;
    float prevInfection = 0f;

    public static event InfectionUpdatedEventHandler OnInfectionPointUpdated;
    public delegate void InfectionUpdatedEventHandler(InfectionPoint point);

    public void Infect(Virus virus)
    {
        if (this.virus != null) return;
        
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

    void UpdateInfectionPoint()
    {
        if (virus != null)
        {
            Infection *= virus.infectionRate;
            InfectAdjacent();
        }
    }

    void InfectAdjacent()
    {
        if (Random.value > adjacentTravelChance) return;
        
        if (adjacents == null)
        {
            adjacents = world.GetAdjacentInfectionPoints(this);
        }
        int travelPoint = Random.Range(0, adjacents.Count - 1);
        adjacents[travelPoint].Infect(virus);
    }

    public void Start()
    {
        InvokeRepeating("UpdateInfectionPoint", 0f, updateInterval);
    }
    
}
