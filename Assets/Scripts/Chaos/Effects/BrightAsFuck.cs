﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [EffectGroup("chaos.bright", "Light Mode")]
    [Description("Makes things very bright")]
    public abstract class BrightAsFuck : ChaosEffect
    {
        Material sun;
        private void Awake()
        {
            sun = new Material(PrefabManager.Instance.sunMat);
        }

        [ChildEffect("chaos.bright.player", "Lone Light"), ConflictsWith(typeof(Rainbow.Car), typeof(DisableShit.Car), typeof(Ghost))]
        [Description("Makes the player blindingly bright, and it's even worse if you lose traction")]
        public class Player : BrightAsFuck
        {
            Material[][] ogMats;
            Renderer[] rends;
            private void OnEnable()
            {
                rends = car.GetComponentsInChildren<Renderer>();
                ogMats = new Material[rends.Length][];
                for (var i = 0; i < rends.Length; i++)
                {
                    ogMats[i] = rends[i].materials;
                    rends[i].materials = new Material[] { sun };
                }
            }

            private void OnDisable()
            {
                for (var i = 0; i < rends.Length; i++)
                {
                    rends[i].materials = ogMats[i];
                }
            }
        }

        [ChildEffect("chaos.bright.checkpoints", "Golden Rings"), ConflictsWith(typeof(Rainbow.Checkpoints))]
        [Description("Makes the checkpoints very bright")]
        public class Checkpoint : BrightAsFuck
        {
            Material ogMat;
            Renderer[] rends;
            private void OnEnable()
            {
                rends = GameObject.Find("/CheckpointArcs").GetComponentsInChildren<Renderer>();
                ogMat = rends[0].material;
                foreach (var rend in rends)
                {
                    rend.material = sun;
                }
            }

            private void OnDisable()
            {
                foreach (var rend in rends)
                {
                    rend.material = ogMat;
                }
            }
        }

        [ChildEffect("chaos.bright.cones", "Light Boxes")]
        [Description("Makes the cones on Funky Forest glow")]
        public class Cones : BrightAsFuck
        {
            Renderer[] rends;
            Material[] ogMats;
            private void OnEnable()
            {
                rends = GameObject.Find("/Cones").GetComponentsInChildren<Renderer>();
                ogMats = rends[0].materials;
                var newMats = new Material[ogMats.Length];
                for (var i = 0; i < newMats.Length; i++) newMats[i] = sun;
                for (var i = 0; i < rends.Length; i++) rends[i].materials = newMats;
            }

            private void OnDisable()
            {
                for (var i = 0; i < rends.Length; i++) rends[i].materials = ogMats;
            }

            public static bool Valid() => GameState.Instance.map == 1;
        }
    }
}