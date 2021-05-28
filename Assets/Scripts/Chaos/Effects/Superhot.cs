using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.superhot", "Superhot"), ConflictsWith(typeof(TAS))]
    public class Superhot : ChaosEffect
    {
        private void Update()
        {
            if (GameController.Instance.playing && !Pause.Instance.paused) Time.timeScale = Mathf.Clamp(Mathf.Abs(car.rb.velocity.magnitude * 3.6f) / 100f, 0.1f, 1f);
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }
    }
}
