using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chaos
{
    // Thanks to Akuma73 for the idea
    [EffectGroup("chaos.gay", "Rainbow")]
    public abstract class Rainbow : ChaosEffect
    {
        protected Material rainbowMat;

        protected virtual void OnEnable()
        {
            Color.RGBToHSV(rainbowMat.color, out var H, out var S, out var V);
            if (S < 0.5f) S = 0.5f;
            if (V < 0.5f) V = 0.5f;
            rainbowMat.color = Color.HSVToRGB(H, S, V);
        }


        protected virtual void Update()
        {
            Color.RGBToHSV(rainbowMat.color, out var H, out var S, out var V);
            H += Time.deltaTime;
            H %= 1;
            rainbowMat.color = Color.HSVToRGB(H, S, V);
        }

        [ChildEffect("chaos.gay.car", "Star Power")]
        public class Car : Rainbow
        {
            protected override void OnEnable()
            {
                var skin = car.GetComponent<CarSkin>();
                rainbowMat = new Material(skin.materials[GameState.Instance.skin]);
                for (var i = 0; i < skin.skinsToChange[0].myArray.Length; i++)
                {
                    var renderer = skin.renderers[skin.skinsToChange[0].myArray[i++]];
                    var newMats = new Material[renderer.materials.Length];
                    renderer.materials.CopyTo(newMats, 0);
                    newMats[skin.skinsToChange[0].myArray[i++]] = rainbowMat;
                    renderer.materials = newMats;
                }
                base.OnEnable();
            }
            public static bool Valid() => car.GetComponent<CarSkin>().skinsToChange.Length > 2;
        }

        [ChildEffect("chaos.gay.checkpoint", "Actual Rainbows")]
        public class Checkpoints : Rainbow
        {
            protected override void OnEnable()
            {
                var arcs = GameObject.Find("/CheckpointArcs").transform;
                rainbowMat = new Material(arcs.GetChild(0).GetComponent<Renderer>().material);
                for (var i = 0; i < arcs.childCount; i++)
                {
                    arcs.GetChild(i).GetComponent<Renderer>().material = rainbowMat;
                }
                base.OnEnable();
            }
        }

        [ChildEffect("chaos.gay.road", "Rainbow Road")]
        public class Road : Rainbow {
            Renderer rend;
            Material ogMat;
            protected override void OnEnable()
            {
                rend = WorldObjects.Instance.road.GetComponent<Renderer>();
                ogMat = rend.material;
                rainbowMat = new Material(ogMat);
                base.OnEnable();
            }

            private void OnDisable()
            {
                rend.material = ogMat;
            }

            protected override void Update()
            {
                rend.material = rainbowMat;
                base.Update();
            }
        }

        [ChildEffect("chaos.gay.sun", "Disco Party"), ConflictsWith(typeof(DisableShit.Sun))]
        public class Sun : Rainbow
        {
            Color ogColor;
            protected override void OnEnable()
            {
                ogColor = WorldObjects.Instance.sun.color;
            }

            protected override void Update()
            {
                Color.RGBToHSV(WorldObjects.Instance.sun.color, out var H, out var S, out var V);
                H += Time.deltaTime * 0.2f;
                H %= 1;
                WorldObjects.Instance.sun.color = Color.HSVToRGB(H, S, V);
            }

            private void OnDisable()
            {
                WorldObjects.Instance.sun.color = ogColor;
            }
        }
    }
}