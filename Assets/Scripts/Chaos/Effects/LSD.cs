using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Chaos
{
    [Effect("chaos.lsd", "LSD")]
    public class LSD : ChaosEffect
    {
        FloatParameter hue;
        PostProcessVolume volume;
        BoolParameter ao;
        BoolParameter bloom;
        BoolParameter dof;
        BoolParameter motionBlur;
        BoolParameter vignette;

        bool enableVolume;
        private void Awake()
        {
            volume = GameObject.Find("/PP").GetComponent<PostProcessVolume>();
            hue = volume.profile.GetSetting<ColorGrading>().hueShift;
            ao = volume.profile.GetSetting<AmbientOcclusion>().enabled;
            bloom = volume.profile.GetSetting<Bloom>().enabled;
            dof = volume.profile.GetSetting<DepthOfField>().enabled;
            motionBlur = volume.profile.GetSetting<MotionBlur>().enabled;
            vignette = volume.profile.GetSetting<Vignette>().enabled;
        }

        private void OnEnable()
        {
            hue.overrideState = true;
            enableVolume = volume.enabled;
            volume.enabled = true;
            if (!enableVolume)
            {
                ao.value = false;
                bloom.value = false;
                dof.value = false;
                motionBlur.value = false;
                vignette.value = false;
            }
        }
        private void OnDisable()
        {
            hue.overrideState = false;
            volume.enabled = enableVolume;
            if (!enableVolume)
            {
                ao.value = true;
                bloom.value = true;
                dof.value = SaveState.Instance.dof == 1;
                motionBlur.value = SaveState.Instance.motionBlur == 1;
                vignette.value = true;
            }
        }

        private void Update()
        {
            hue.value -= Time.deltaTime * 72f; // returns to 0 after 5 seconds, making the disabling seamless
            if (hue.value < -180f) hue.value += 360f;
        }
    }
}