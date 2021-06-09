using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chaos
{
    // Thanks to Akuma73 for the idea
    [EffectGroup("chaos.gay", "Rainbow", Alignment = EffectInfo.Alignment.Neutral)]
    [Description("Hue shifts objects in the game")]
    public abstract class Rainbow : ChaosEffect
    {
        protected Material rainbowMat;

        protected override void Enable()
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

        [ChildEffect("chaos.gay.car", "Star Power"), ConflictsWith(typeof(Ghost), typeof(BrightAsFuck.Player))]
        [Description("Hue shifts your car's material")]
        public class Car : Rainbow
        {
            CarSkin skin;
            protected override void Enable()
            {
                skin = car.GetComponent<CarSkin>();
                rainbowMat = new Material(skin.materials[GameState.Instance.skin]);
                base.Enable();
            }

            protected override void Update()
            {
                base.Update();
                for (var i = 0; i < skin.skinsToChange[0].myArray.Length; i++) {
                    var renderer = skin.renderers[skin.skinsToChange[0].myArray[i++]];
                    var newMats = new Material[renderer.materials.Length];
                    renderer.materials.CopyTo(newMats, 0);
                    newMats[skin.skinsToChange[0].myArray[i++]] = rainbowMat;
                    renderer.materials = newMats;
                }
            }
            public static bool Valid() => car.GetComponent<CarSkin>().skinsToChange.Length > 2;
        }

        [ChildEffect("chaos.gay.checkpoint", "Actual Rainbows")]
        [Description("Hue shifts checkpoint arcs")]
        public class Checkpoints : Rainbow
        {
            protected override void Enable()
            {
                var arcs = GameObject.Find("/CheckpointArcs").transform;
                rainbowMat = new Material(arcs.GetChild(0).GetComponent<Renderer>().material);
                for (var i = 0; i < arcs.childCount; i++)
                {
                    arcs.GetChild(i).GetComponent<Renderer>().material = rainbowMat;
                }
                base.Enable();
            }
        }

        [ChildEffect("chaos.gay.road", "Rainbow Road")]
        [Description("Hue shifts the road texture")]
        public class Road : Rainbow
        {
            Renderer rend;
            Material ogMat;
            protected override void Enable()
            {
                rend = WorldObjects.Instance.road.GetComponent<Renderer>();
                ogMat = rend.material;
                rainbowMat = new Material(ogMat);
                base.Enable();
            }

            protected override void Disable()
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
        [Description("Hue shifts the sun color")]
        public class Sun : Rainbow
        {
            Color ogColor;
            protected override void Enable()
            {
                ogColor = WorldObjects.Instance.sun.color;
            }
            protected override void Disable()
            {
                WorldObjects.Instance.sun.color = ogColor;
            }

            protected override void Update()
            {
                Color.RGBToHSV(WorldObjects.Instance.sun.color, out var H, out var S, out var V);
                H += Time.deltaTime * 0.2f;
                H %= 1;
                WorldObjects.Instance.sun.color = Color.HSVToRGB(H, S, V);
            }

        }
    }
}