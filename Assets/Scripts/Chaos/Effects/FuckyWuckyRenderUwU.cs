using UnityEngine;

namespace Chaos
{
    [Effect("chaos.knightbus", "Mind your head", EffectInfo.Alignment.Neutral), ConflictsWith(typeof(DisableShit.Car))] // Thanks to Dit0h for the name
    [Description("Stretches your car in a random direction")]
    public class FuckyWuckyRenderUwU : ChaosEffect
    {
        private Vector3 og;
        private Vector3 target;
        private Transform victim;

        protected override void Enable()
        {
            victim = car.transform.GetChild(0);
            og = victim.localScale;
        }

        protected override void Disable() => victim.localScale = og;

        private float time;

        private void Update()
        {
            if (time == 0f) target = Vector3.Scale(og, new Vector3(Random.Range(-3f, 3f), Random.Range(-1f, 3f), Random.Range(-3f, 3f)));
            time += Time.deltaTime;
            victim.localScale = Vector3.Lerp(og, target, Mathf.PingPong(time, 2.5f) / 2.5f);
            if (time > 5f) time = 0f;
        }
    }
}
