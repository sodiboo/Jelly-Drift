using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [EffectGroup("chaos.time", "Time Manipulation")]
    class TimeManipulation : ChaosEffect
    {

        [Effect("chaos.time.tas", "TAS")]
        [Description("Makes your car always appear to go at 50 ku/h")]
        class TAS : TimeManipulation
        {
            private void OnDisable()
            {
                Time.timeScale = 1f;
            }

            private void FixedUpdate()
            {
                if (car.rb.velocity.magnitude < 0.1f) Time.timeScale = 1f;
                else Time.timeScale = 14f / car.rb.velocity.magnitude;
            }
        }

        [Effect("chaos.time.superhot", "Superhot")]
        [Description("Time only moves as fast as you move")]
        public class Superhot : TimeManipulation
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
}