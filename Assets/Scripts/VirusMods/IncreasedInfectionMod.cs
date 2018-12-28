public class IncreasedInfectionMod : VirusMod
{
    public float infectionRateMultiplier;
    Virus virus;

    public override void Apply(Virus virus)
    {
        this.virus = virus;
        virus.infectionRate *= infectionRateMultiplier;
    }

    public override void Remove(Virus virus)
    {
        virus.infectionRate /= infectionRateMultiplier;
    }
}
