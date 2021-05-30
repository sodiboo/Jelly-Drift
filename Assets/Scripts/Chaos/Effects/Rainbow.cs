using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Chaos
{
    [Effect("chaos.gay", "Rainbow")] // Thanks to Akuma73 for the idea
    public class Rainbow : ChaosEffect
    {
        Material rainbowMat;
        Material road;
        Material ogRoad;

        private void OnEnable()
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

            Color.RGBToHSV(rainbowMat.color, out var H, out var S, out var V);
            if (S < 0.5f) S = 0.5f;
            if (V < 0.5f) V = 0.5f;
            rainbowMat.color = Color.HSVToRGB(H, S, V);
            var arcs = GameObject.Find("/CheckpointArcs").transform;
            for (var i = 0; i < arcs.childCount; i++)
            {
                arcs.GetChild(i).GetComponent<Renderer>().materials = new Material[] { rainbowMat };
            }
            var rend = WorldObjects.Instance.road.GetComponent<MeshRenderer>();
            ogRoad = rend.material;
            road = new Material(rend.material);
            rend.material = road;
            Color.RGBToHSV(road.color, out H, out S, out V);
            if (S < 0.5f) S = 0.5f;
            if (V < 0.5f) V = 0.5f;
            road.color = Color.HSVToRGB(H, S, V);
        }

        public static bool Valid() => car.GetComponent<CarSkin>().skinsToChange.Length > 2;

        private void Update()
        {
            Color.RGBToHSV(rainbowMat.color, out var H, out var S, out var V);
            H += Time.deltaTime;
            H %= 1;
            rainbowMat.color = Color.HSVToRGB(H, S, V);
            if (road != null)
            {
                Color.RGBToHSV(road.color, out H, out S, out V);
                H -= Time.deltaTime;
                H += 1;
                H %= 1;
                road.color = Color.HSVToRGB(H, S, V);
            }
        }

        private void OnDisable()
        {
            WorldObjects.Instance.road.GetComponent<MeshRenderer>().material = ogRoad;
        }
    }
}