using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos
{
    [Effect("chaos.random.skin", "Random Skin"), Impulse]
    public class RandomSkin : ChaosEffect
    {
        private void Awake()
        {
            var skin = car.GetComponent<CarSkin>();
            var current = GameState.Instance.skin;
            var rand = Random.Range(0, skin.skinsToChange.Length - 1);
            if (current <= rand) rand++;
            GameState.Instance.skin = rand;
            skin.SetSkin(rand);
        }
    }
}