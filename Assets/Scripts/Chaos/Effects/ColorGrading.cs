using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using CG = UnityEngine.Rendering.PostProcessing.ColorGrading;

namespace Chaos
{
    [EffectGroup("chaos.colorgrading", "Color Grading", SeparateCheats = true)]
    public abstract class ColorGrading : ChaosEffect
    {
        PostProcessVolume volume;
        BoolParameter ao;
        BoolParameter bloom;
        BoolParameter dof;
        BoolParameter motionBlur;
        BoolParameter vignette;

        bool enableVolume;
        protected virtual void Awake()
        {
            volume = GameObject.Find("/PP").GetComponent<PostProcessVolume>();
            ao = volume.profile.GetSetting<AmbientOcclusion>().enabled;
            bloom = volume.profile.GetSetting<Bloom>().enabled;
            dof = volume.profile.GetSetting<DepthOfField>().enabled;
            motionBlur = volume.profile.GetSetting<MotionBlur>().enabled;
            vignette = volume.profile.GetSetting<Vignette>().enabled;
        }

        protected virtual void OnEnable()
        {
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

        protected virtual void OnDisable()
        {
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

        [Effect("chaos.nightvis", "Night Vision")]
        [Description("Deletes the blue and red color channels, doesn't require ")]
        public class NightVision : ColorGrading
        {
            FloatParameter red;
            FloatParameter blue;
            protected override void Awake()
            {
                base.Awake();
                var grading = volume.profile.GetSetting<CG>();
                (red = grading.mixerRedOutRedIn).value = 0f;
                (blue = grading.mixerBlueOutBlueIn).value = 0f;
            }
            protected override void OnEnable()
            {
                red.overrideState = true;
                red.overrideState = true;
                base.OnEnable();
            }

            protected override void OnDisable()
            {
                red.overrideState = false;
                blue.overrideState = false;
                base.OnDisable();
            }
        }

        [Effect("chaos.lsd", "LSD")]
        [Description("Hue shifts your entire view")]
        public class LSD : ColorGrading
        {
            FloatParameter hue;
            protected override void Awake()
            {
                base.Awake();
                hue = volume.profile.GetSetting<CG>().hueShift;
            }

            protected override void OnEnable()
            {
                hue.overrideState = true;
                base.OnEnable();
            }
            protected override void OnDisable()
            {
                hue.overrideState = false;
                base.OnDisable();
            }

            private void Update()
            {
                hue.value -= Time.deltaTime * 72f; // returns to 0 after 5 seconds, making the disabling seamless
                if (hue.value < -180f) hue.value += 360f;
            }
        }
    }
}