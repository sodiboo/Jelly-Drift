using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Chaos
{
    [Effect("chaos.nightvis", "Night Vision")]
    class NightVision : ChaosEffect
    {
        ColorGrading grading;
        private void Awake()
        {
            grading = GameObject.Find("/PP").GetComponent<PostProcessVolume>().profile.GetSetting<ColorGrading>();
            grading.mixerRedOutRedIn.value = 0f;
            grading.mixerBlueOutBlueIn.value = 0f;
        }
        private void OnEnable()
        {
            grading.mixerRedOutRedIn.overrideState = true;
            grading.mixerBlueOutBlueIn.overrideState = true;
        }

        private void OnDisable()
        {
            grading.mixerRedOutRedIn.overrideState = false;
            grading.mixerBlueOutBlueIn.overrideState = false;
        }
    }
}
