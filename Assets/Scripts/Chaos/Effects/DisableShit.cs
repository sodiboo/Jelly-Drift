using UnityEngine;

namespace Chaos
{
    [EffectGroup("chaos.disable", "Disable Shit")]
    public abstract class DisableShit : ChaosEffect
    {
        protected abstract Object[] things { get; }

        protected override void Enable()
        {
            switch (things)
            {
                case Renderer[] renderers: foreach (var el in renderers) el.enabled = false; break;
                case Behaviour[] behaviours: foreach (var el in behaviours) el.enabled = false; break;
                case GameObject[] objects: foreach (var el in objects) el.SetActive(false); break;
            }
        }

        protected override void Disable()
        {
            switch (things)
            {
                case Renderer[] renderers: foreach (var el in renderers) el.enabled = true; break;
                case Behaviour[] behaviours: foreach (var el in behaviours) el.enabled = true; break;
                case GameObject[] objects: foreach (var el in objects) el.SetActive(true); break;
            }
        }

        [Effect("chaos.disable.car", "Wait, where did it go?", EffectInfo.Alignment.Neutral)]
        [Description("Makes your car invisible")]
        public class Car : DisableShit
        {
            protected override Object[] things => car.GetComponentsInChildren<Renderer>();
        }

        [Effect("chaos.disable.world", "Where did everything go?", EffectInfo.Alignment.Neutral)] // Thanks to WoodComet for the idea, thanks to ChaosModV for the name
        [Description("Makes the road and terrain invisible")]
        public class World : DisableShit
        {
            protected override Object[] things => new Renderer[] {
                WorldObjects.Instance.road.GetComponent<Renderer>(),
                WorldObjects.Instance.terrain.GetComponent<Renderer>(),
            };
        }

        [Effect("chaos.disable.sun", "Dark Mode", EffectInfo.Alignment.Neutral)] // Thanks to WoodComet for the idea
        [Description("Turns off the sun")]
        public class Sun : DisableShit
        {
            protected override Object[] things => new Behaviour[] { WorldObjects.Instance.sun };
        }

        [Effect("chaos.disable.road", "Nonexistent Road", EffectInfo.Alignment.Neutral)] // Thanks to ProfessorEmu for the idea, i think
        [Description("Deletes the road")]
        public class Road : DisableShit
        {
            protected override Object[] things => new GameObject[] { WorldObjects.Instance.road };
        }
    }
}