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
    public float adjacentTravelChance = .1f; // means x% of population will travel per turn
    public float airplaneTravelChance = 2.5f; // means x% of population will fly per turn
    public List<int> adjacentInfectionPointIndexes = new List<int>();
    public float temperatureFactor;
    public bool isAirport;
    public Airplane airplanePrefab;
    public long totalPopulation;
    public long infectedPopulation;
    public long infectedPopulationDelta; //the change in infected population during the most recent update

    float prevInfection = 0f;

    public static event InfectionUpdatedEventHandler OnInfectionPointUpdated;
    public delegate void InfectionUpdatedEventHandler(InfectionPoint point);

    public void Start()
    {
        InvokeRepeating("UpdateInfectionPoint", Random.value, updateInterval);
    }

    public void Infect(Virus virus)
    {
        if (virus == null || this.virus != null) return;

        this.virus = virus;
        Infection = .0001f;
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
                infectedPopulation = (long)(totalPopulation * infection);
                infectedPopulationDelta = infectedPopulation - (long)(totalPopulation * prevInfection);
                OnInfectionPointUpdated(this);
                infectedPopulationDelta = 0;
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
            float healthCareReduction = 1f - healthCare;
            float temperatureReduction = 1f - temperatureFactor;

            float infectionIncrease = virus.infectionRate * temperatureReduction * healthCareReduction;
            
            Infection += Infection * infectionIncrease;
            InfectAdjacent();            
        }
    }

    void InfectAdjacent()
    {
        float adjacentRoll = Random.Range(0f, population);
        if (adjacentRoll >= adjacentTravelChance * population * infection) return;
        
        int travelPoint = Random.Range(0, adjacentInfectionPointIndexes.Count - 1);
        int y = adjacentInfectionPointIndexes[travelPoint] / 360;
        int x = adjacentInfectionPointIndexes[travelPoint] % 360;
        InfectionPoint adjacentPoint = world.infectionPoints[y][x];
        if (adjacentPoint != null)
        {
            adjacentPoint.Infect(virus);
        }
    }

    void LaunchAirplane()
    {
        float airplaneRoll = Random.Range(0f, population);
        if (airplaneRoll >= airplaneTravelChance * population) return;

        InfectionPoint destination = world.GetRandomAirport();
        if (destination != null)
        {
            Airplane airplane = Instantiate(airplanePrefab, world.globe.Earth.transform);
            airplane.globe = world.globe;
            airplane.source = this;
            airplane.destination = destination;
            //Debug.Log("Airplane has taken off from " + countryName + " bound for " + destination.countryName);
            if (Random.value < infection && virus != null)
            {
                airplane.Infect(virus);
                Debug.Log("INFECTED! Airplane");
            }
            airplane.TakeOff();
        }
    }    
}
