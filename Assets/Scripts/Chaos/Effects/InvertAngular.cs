namespace Chaos
{
    [Effect("chaos.forces.invert_angular", "Go left! No, go right, go right!", EffectInfo.Alignment.Bad), Impulse] // Quote from Wheatley
    [Description("Inverts your angular velocity and amplifies it (-5x)")]
    internal class InvertAngular : ChaosEffect
    {
        protected override void Enable() => car.rb.angularVelocity *= -5f;
    }
}