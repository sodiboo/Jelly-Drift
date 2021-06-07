﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.lowfps", "Console Experience")] // Thanks to ChaosModV for the name and idea
    [Description("Sets the target framerate to 15fps")]
    public class LowFPS : ChaosEffect
    {
        private void OnEnable()
        {
            Application.targetFrameRate = 15;
        }
        private void OnDisable()
        {
            Application.targetFrameRate = -1;
        }
    }
}