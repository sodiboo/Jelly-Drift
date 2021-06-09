namespace Chaos
{
    [Effect("chaos.sliproad", "Offroad = Road", EffectInfo.Alignment.Neutral), ConflictsWith(typeof(Speed))] // Thanks to Akuma73 for the name
    [Description("Inverts the check for being on/off road and makes your car a bit stronger so it's advantageous to go offroad")]
    public class InvertTerrain : ChaosEffect
    {
        public static bool value; // implementation in Suspension.NewSuspension
        protected override void Enable()
        {
            value = true;
            car.engineForce *= 2f;
        }

        protected override void Disable()
        {
            car.engineForce /= 2f;
            value = false;
        }
    }
}