using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    public abstract class DisableShit : ChaosEffect
    {
        protected abstract Object[] things { get; }

        private void OnEnable()
        {
            switch (things) {
                case Renderer[] renderers:  foreach (var el in renderers) el.enabled = false; break;
                case Behaviour[] behaviours: foreach (var el in behaviours) el.enabled = false; break;
                case GameObject[] objects: foreach (var el in objects) el.SetActive(false); break;
            }
        }

        private void OnDisable()
        {
            switch (things)
            {
                case Renderer[] renderers: foreach (var el in renderers) el.enabled = true; break;
                case Behaviour[] behaviours: foreach (var el in behaviours) el.enabled = true; break;
                case GameObject[] objects: foreach (var el in objects) el.SetActive(true); break;
            }
        }

        [Effect("chaos.disable.car", "Wait, where did it go?")]
        public class Car : DisableShit
        {
            protected override Object[] things => car.GetComponentsInChildren<Renderer>();
        }

        [Effect("chaos.disable.world", "Where did everything go?")] // Thanks to WoodComet for the idea, thanks to ChaosModV for the name
        public class World : DisableShit
        {
            protected override Object[] things => new Renderer[] {
                WorldObjects.Instance.road.GetComponent<Renderer>(),
                WorldObjects.Instance.terrain.GetComponent<Renderer>(),
            };
        }

        [Effect("chaos.disable.sun", "Dark Mode")] // Thanks to WoodComet for the idea
        public class Sun : DisableShit
        {
            protected override Object[] things => new Behaviour[] { WorldObjects.Instance.sun };
        }

        [Effect("chaos.disable.road", "Nonexistent Road")] // Thanks to ProfessorEmu for the idea, i think
        public class Road : DisableShit
        {
            protected override Object[] things => new GameObject[] { WorldObjects.Instance.road };
        }
    }
}