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
    public string regionName;
    public int vertIndex;
    public Virus virus;
    public World world;
    public float updateInterval = 1.0f;
    public float adjacentTravelChance = .1f;
    public float airplaneTravelChance = .025f;
    public List<int> adjacentInfectionPointIndexes = new List<int>();
    public float temperatureFactor;
    public bool isAirport;
    public Airplane airplanePrefab;

    float prevInfection = 0f;

    public static event InfectionUpdatedEventHandler OnInfectionPointUpdated;
    public delegate void InfectionUpdatedEventHandler(InfectionPoint point);

    public void Infect(Virus virus)
    {
        if (virus == null || this.virus != null) return;

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
        if (isAirport)
        {
            LaunchAirplane();
        }

        if (virus != null)
        {
            float healthCareReduction = ((virus.infectionRate - 1f) * healthCare);
            float infectionIncrease = virus.infectionRate - healthCareReduction;
            infectionIncrease -= ((infectionIncrease - 1f) * temperatureFactor);
            Infection *= infectionIncrease;
            InfectAdjacent();            
        }
    }

    void InfectAdjacent()
    {
        float adjacentRoll = Random.value;
        if (adjacentRoll > adjacentTravelChance) return;
        
        int travelPoint = Random.Range(0, adjacentInfectionPointIndexes.Count - 1);
        int y = adjacentInfectionPointIndexes[travelPoint] / 360;
        int x = adjacentInfectionPointIndexes[travelPoint] % 360;
        InfectionPoint adjacentPoint = world.infectionPoints[y][x];
        if (adjacentPoint != null)
        {
            if (adjacentRoll < adjacentTravelChance * adjacentPoint.temperatureFactor)
            {
                adjacentPoint.Infect(virus);
            }
        }
    }

    void LaunchAirplane()
    {
        float airplaneRoll = Random.value;
        if (airplaneRoll > airplaneTravelChance) return;

        InfectionPoint destination = world.GetRandomAirport();
        if (destination != null)
        {
            Airplane airplane = Instantiate(airplanePrefab, world.globe.Earth.transform);
            airplane.globe = world.globe;
            airplane.source = this;
            airplane.destination = destination;
            if (virus != null)
            {
                float infectionChance = ((virus.infectionRate - 1f) * healthCare);
                infectionChance -= ((infectionChance - 1f) * temperatureFactor);
                if (Random.value < infectionChance)
                {
                    airplane.Infect(virus);
                    Debug.Log("INFECTED! Airplane has taken off from " + countryName + " bound for " + destination.countryName);
                }
            }
            airplane.TakeOff();
        }
    }

    public void Start()
    {
        InvokeRepeating("UpdateInfectionPoint", Random.value, updateInterval);
        adjacentTravelChance = adjacentTravelChance - (adjacentTravelChance * healthCare);        
    }
    
}
