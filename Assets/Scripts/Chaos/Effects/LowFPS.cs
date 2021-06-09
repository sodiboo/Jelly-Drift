using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.lowfps", "Console Experience", EffectInfo.Alignment.Bad)] // Thanks to ChaosModV for the name and idea
    [Description("Sets the target framerate to 15fps")]
    public class LowFPS : ChaosEffect
    {
        protected override void Enable()
        {
            Application.targetFrameRate = 15;
        }
        protected override void Disable()
        {
            Application.targetFrameRate = -1;
        }
    }
}