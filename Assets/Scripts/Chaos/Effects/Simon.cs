﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.simon", "Dani Says"), ConflictsWith(typeof(Punishment)), HideInCheatGUI]
    public class Simon : ChaosEffect
    {
        private void OnEnable()
        {
            StartCoroutine(StartSimon());
        }

        bool invertSimon;
        SimonAction simon;
        bool enforceSimon;

        enum SimonAction
        {
            Accelerate,
            Decelerate,
            Left,
            Right,
        }

        IEnumerator StartSimon()
        {
            simon = (SimonAction)Random.Range(0, System.Enum.GetValues(typeof(SimonAction)).Length);
            invertSimon = Random.value > 0.5f;
            Time.timeScale *= 0.2f;
            var split = Instantiate(PrefabManager.Instance.splitUi).GetComponent<SplitUi>();
            split.transform.SetParent(UIManager.Instance.splitPos);
            split.transform.localPosition = Vector3.zero;
            string name = "";
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

        void Update()
        {
            if (enforceSimon && invertSimon == FollowsSimon())
            {
                car.rb.constraints = RigidbodyConstraints.FreezeAll;
                enforceSimon = false;
                ChaosController.Instance.riggedEffect = ChaosController.effectMap[typeof(Punishment)];
            }
        }

        bool FollowsSimon()
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

        [Effect("chaos.simon.punishment", "Punishment"), HideInCheatGUI]
        public class Punishment : ChaosEffect
        {
            public static bool Valid() => false;
            private void OnDisable()
            {
                car.rb.constraints = RigidbodyConstraints.None;
            }
        }
    }
}