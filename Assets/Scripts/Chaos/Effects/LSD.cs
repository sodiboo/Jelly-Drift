using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Chaos
{
    [Effect("chaos.lsd", "LSD")]
    class LSD : ChaosEffect
    {
        FloatParameter hue;
        private void Awake()
        {
            hue = GameObject.Find("/PP").GetComponent<PostProcessVolume>().profile.GetSetting<ColorGrading>().hueShift;
        }

        private void OnEnable()
        {
            hue.overrideState = true;
        }
        private void OnDisable()
        {
            hue.overrideState = false;
        }

        private void Update()
        {
            hue.value -= Time.deltaTime * 72f; // returns to 0 after 5 seconds, making the disabling seamless
            if (hue.value < -180f) hue.value += 360f;
        }
    }
}