using UnityEngine;

namespace Chaos
{
    [EffectGroup("chaos.time", "Time Manipulation")]
    internal class TimeManipulation : ChaosEffect
    {
        protected override void Disable() => Time.timeScale = 1f;

        [Effect("chaos.time.tas", "TAS", EffectInfo.Alignment.Neutral)]
        [Description("Makes your car always appear to go at 50 ku/h")]
        private class TAS : TimeManipulation
        {
            private void FixedUpdate()
            {
                if (car.rb.velocity.magnitude < 0.1f) Time.timeScale = 1f;
                else Time.timeScale = 14f / car.rb.velocity.magnitude;
            }
        }

        [Effect("chaos.time.superhot", "Superhot", EffectInfo.Alignment.Neutral)]
        [Description("Time only moves as fast as you move")]
        public class Superhot : TimeManipulation
        {
            private void Update()
            {
                if (GameController.Instance.playing && !Pause.Instance.paused) Time.timeScale = Mathf.Clamp(Mathf.Abs(car.rb.velocity.magnitude * 3.6f) / 100f, 0.1f, 1f);
            }
        }
    }
}