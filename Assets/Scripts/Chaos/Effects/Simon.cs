using System.Collections;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.simon", "Dani Says", EffectInfo.Alignment.Bad), ConflictsWith(typeof(Punishment)), HideInCheatGUI]
    [Description("Forces you to do what Dani says, or not do what he doesn't say")]
    public class Simon : ChaosEffect
    {
        protected override void Enable() => StartCoroutine(StartSimon());

        private bool invertSimon;
        private SimonAction simon;
        private bool enforceSimon;

        private enum SimonAction
        {
            Accelerate,
            Decelerate,
            Left,
            Right,
        }

        private IEnumerator StartSimon()
        {
            simon = (SimonAction)Random.Range(0, System.Enum.GetValues(typeof(SimonAction)).Length);
            invertSimon = Random.value > 0.5f;
            Time.timeScale *= 0.2f;
            var split = Instantiate(PrefabManager.Instance.splitUi).GetComponent<SplitUi>();
            split.transform.SetParent(UIManager.Instance.splitPos);
            split.transform.localPosition = Vector3.zero;
            var name = "";
            switch (simon)
            {
                case SimonAction.Accelerate: name = "forwards"; break;
                case SimonAction.Decelerate: name = "backwards"; break;
                case SimonAction.Right: name = "right"; break;
                case SimonAction.Left: name = "left"; break;
            }
            split.SetSplit(invertSimon ? $"Press {name}" : $"Dani says press {name}");
            yield return new WaitForSeconds(1f);

            enforceSimon = true;
            Time.timeScale *= 5f;
        }

        private void Update()
        {
            if (enforceSimon && invertSimon == FollowsSimon())
            {
                car.rb.constraints = RigidbodyConstraints.FreezeAll;
                enforceSimon = false;
                ChaosController.Instance.riggedEffect = ChaosController.effectMap[typeof(Punishment)];
            }
        }

        private bool FollowsSimon()
        {
            switch (simon)
            {
                case SimonAction.Accelerate: return car.throttle > 0f;
                case SimonAction.Decelerate: return car.throttle < 0f;
                case SimonAction.Right: return car.steering > 0f;
                case SimonAction.Left: return car.steering < 0f;
                default: return false;
            }
        }

        [Effect("chaos.simon.punishment", "Punishment", default(EffectInfo.Alignment)), HideInCheatGUI]
        [Description("Punishes you for failing simon says, by locking your position and rotation")]
        public class Punishment : ChaosEffect
        {
            public static bool Valid() => false;
            protected override void Disable() => car.rb.constraints = RigidbodyConstraints.None;
        }
    }
}