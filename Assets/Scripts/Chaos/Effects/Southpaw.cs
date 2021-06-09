namespace Chaos
{
    [Effect("chaos.controls.southpaw", "Southpaw", EffectInfo.Alignment.Bad)] // Thanks to Dit0h for the name and idea
    [Description("Makes your controls right handed (IJKL)")]
    internal class Southpaw : ChaosEffect
    {
        protected override void Enable() => InputManager.Instance.layout = InputManager.Layout.Southpaw;

        protected override void Disable() => InputManager.Instance.layout = InputManager.Layout.Car;
    }
}
