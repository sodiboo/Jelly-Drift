using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Alignment = EffectInfo.Alignment;
using Random = UnityEngine.Random;

namespace Chaos
{
    [EffectGroup("chaos.tasks", "Tasks"), HideInCheatGUI]
    public abstract class TaskEffect : ChaosEffect
    {
        protected static Alignment align = default;
        private static IEnumerable<EffectInfo> Effects()
        {
            foreach (var effect in ChaosController.effects)
            {
                if (effect.isChild) continue;
                if (effect == ChaosController.effectMap[typeof(TaskEffect)]) continue;
                yield return effect;
            }
        }

        private static Func<EffectInfo, bool> HasAlignment(Alignment alignment) => effect =>
        {
            if (effect.effectType == EffectInfo.EffectType.ExclusiveGroup) return effect.children.Any(HasAlignment(alignment));
            return alignment.HasFlag(effect.alignment);
        };

        private static bool IsValid(EffectInfo effect)
        {
            if (effect.effectType == EffectInfo.EffectType.ExclusiveGroup) return effect.children.Any(IsValid);
            return effect.valid;
        }

        private static EffectInfo RandomFrom(EffectInfo[] effects, Alignment alignment)
        {
            var effect = effects[Random.Range(0, effects.Length)];
            if (effect.effectType == EffectInfo.EffectType.ExclusiveGroup)
            {
                var children = effect.children.Where(IsValid).Where(HasAlignment(alignment)).ToArray();
                return children[Random.Range(0, children.Length)];
            }
            return effect;
        }

        protected static EffectInfo Effect(Alignment alignment) => RandomFrom(Effects().Where(IsValid).Where(HasAlignment(alignment)).ToArray(), alignment);

        protected EffectInfo completionEffect;
        protected ChaosEffect activeCompletionEffect;
        protected List<ChaosEffect> activeCompletionChildren;
        protected EffectInfo failureEffect;
        protected ChaosEffect activeFailureEffect;
        protected List<ChaosEffect> activeFailureChildren;

        protected override void Awake()
        {
            completionEffect = Effect(Alignment.Good);
            failureEffect = Effect(Alignment.Bad);
            AddEffect(completionEffect, out activeCompletionEffect, out activeCompletionChildren);
            AddEffect(failureEffect, out activeFailureEffect, out activeFailureChildren);
            base.Awake();
        }

        private void AddEffect(EffectInfo effect, out ChaosEffect active, out List<ChaosEffect> children)
        {
            if (effect.effectType == EffectInfo.EffectType.MultiGroup)
            {
                children = effect.children.Where(IsValid).Select(child => (ChaosEffect)gameObject.AddComponent(child.type)).ToList();
                active = null;
            }
            else
            {
                children = null;
                active = (ChaosEffect)gameObject.AddComponent(effect.type);
            }
        }

        private string Format(EffectInfo info, ChaosEffect effect)
        {
            var result = ChaosController.Instance.globalFormatting.Aggregate(info.name,
            (name, format) => name.Replace($"@{format.Key}", format.Value()));
            var parameters = effect?.CustomParameters();
            if (parameters != null) result = string.Format(result, parameters);
            return result;
        }


        public override object[] CustomParameters() => new object[]
        {
            $"<color={(CompletedTask() ? "#35ebe8" : "white")}>{Format(completionEffect, activeCompletionEffect)}</color>",
            $"<color={(CompletedTask() ? "white" : "#35ebe8")}>{Format(failureEffect, activeFailureEffect)}</color>",
        };

        public string CheckTask(out EffectInfo effect, out ChaosEffect active, out List<ChaosEffect> children)
        {
            if (CompletedTask())
            {
                if (activeFailureEffect != null) Destroy(activeFailureEffect);
                if (activeFailureChildren != null) activeFailureChildren.ForEach(Destroy);
                effect = completionEffect;
                if ((active = activeCompletionEffect) != null) active.enabled = true;
                if ((children = activeCompletionChildren) != null) children.ForEach(child => child.enabled = true);
                return Format(completionEffect, activeCompletionEffect);
            }
            if (activeCompletionEffect != null) Destroy(activeCompletionEffect);
            if (activeCompletionChildren != null) activeCompletionChildren.ForEach(Destroy);
            effect = failureEffect;
            if ((active = activeFailureEffect) != null) active.enabled = true;
            if ((children = activeFailureChildren) != null) children.ForEach(child => child.enabled = true);
            return Format(failureEffect, activeFailureEffect);
        }

        protected abstract bool CompletedTask();
        protected abstract string Completion();

        private void LateUpdate() => ChaosController.Instance.text.text += $"\n\n<size=32>{Completion()}";

        protected static string LinearColor(float progress)
        {
            var color = Color.Lerp(new Color(0xEB, 0x35, 0x38), new Color(0x38, 0xEB, 0x35), progress);
            return $"<color=#{Mathf.RoundToInt(color.r):X2}{Mathf.RoundToInt(color.g):X2}{Mathf.RoundToInt(color.b):X2}>";
        }

        [Effect("chaos.tasks.drift", "Drift for {0} seconds to get \"{1}\" or else \"{2}\"", default)]
        public class DriftTask : TaskEffect
        {
            private float target;
            protected override void Awake()
            {
                target = Random.Range(1f, 3f);
                base.Awake();
            }

            private float completed;

            private void Update()
            {
                if (car.drifting) completed += Time.deltaTime;
            }

            protected override bool CompletedTask() => completed >= target;
            protected override string Completion() => $"{LinearColor(completed / target)}{completed:0.0}/{target:0.0}";
            public override object[] CustomParameters() => new object[]
            {
                target.ToString("0.0"),
            }.Concat(base.CustomParameters()).ToArray();
        }

        [Effect("chaos.tasks.speed", "Reach {0} to get \"{1}\" or else \"{2}\"", default)]
        public class SpeedTask : TaskEffect
        {
            private float target;
            private readonly float unitMultiplier = SaveState.Instance.speedometer == 1 ? 1 : 3.6f;
            protected override void Awake()
            {
                target = Random.Range(10f, 30f);
                base.Awake();
            }

            private float completed = 0f;

            private void FixedUpdate()
            {
                completed = Mathf.Max(car.rb.velocity.magnitude, completed);
                if (completed < target) completed = Mathf.Lerp(completed, car.rb.velocity.magnitude, Time.deltaTime);
            }

            protected override bool CompletedTask() => completed >= target;
            protected override string Completion() => $"{LinearColor(completed / target)}{completed * unitMultiplier:0.0}/{target * unitMultiplier:0.0}";

            private static Func<float, string>[] displays => new Func<float, string>[] {
                (amount) => $"{amount * 3.6f:0.0} speed",
                (amount) => $"{amount:0.0} u/s",
                (amount) => $"{amount * 3.6f:0.0} ku/h",
            };

            public override object[] CustomParameters() => new object[]
            {
                displays[SaveState.Instance.speedometer](target),
            }.Concat(base.CustomParameters()).ToArray();
        }
    }
}