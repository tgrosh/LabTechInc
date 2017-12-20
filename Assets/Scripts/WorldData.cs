[System.Serializable]
public class WorldData {
    public CountryData[] Countries;
}

[System.Serializable]
public class CountryData
{
    public string Name;
    public CountryPoint[] Points;
}

[System.Serializable]
public class CountryPoint
{
    public int x;
    public int y;
    public float population;
    public float infection;
    public float healthCare;
}