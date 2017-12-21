using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionPoint: MonoBehaviour
{
    public int x;
    public int y;
    public float population;
    public float infection;
    public float healthCare;
    public string countryName;
    public int vertIndex;
    public Virus virus;
    public World world;
    public float updateInterval = 1.0f;
    public float adjacentTravelChance = .1f; //.001
    public List<int> adjacentInfectionPointIndexes = new List<int>();
    
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
            float infectionIncrease = virus.infectionRate - ((virus.infectionRate - 1f) * healthCare);
            Infection *= infectionIncrease;
            InfectAdjacent();
        }
    }

    void InfectAdjacent()
    {
        if (Random.value > adjacentTravelChance) return;
        
        int travelPoint = Random.Range(0, adjacentInfectionPointIndexes.Count - 1);
        int y = adjacentInfectionPointIndexes[travelPoint] / 360;
        int x = adjacentInfectionPointIndexes[travelPoint] % 360;
        InfectionPoint adjacentPoint = world.infectionPoints[y][x];
        if (adjacentPoint != null)
        {
            adjacentPoint.Infect(virus);
        }
    }

    public void Start()
    {
        InvokeRepeating("UpdateInfectionPoint", Random.value, updateInterval);
        adjacentTravelChance = adjacentTravelChance - (adjacentTravelChance * healthCare);
    }
    
}
