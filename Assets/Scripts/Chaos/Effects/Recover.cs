using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.recover", "Are you sure you got that checkpoint?", EffectInfo.Alignment.Bad), Impulse, HideInCheatGUI] // Thanks to Dit0h for name and idea
    [Description("Forces you to teleport to the last checkpoint")]
    class Recover : ChaosEffect
    {
        protected override void Enable()
        {
            GameController.Instance.Recover();
        }
    }
}