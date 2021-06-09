using UnityEngine;

namespace Chaos
{
    [Effect("chaos.random.skin", "Random Skin", EffectInfo.Alignment.Neutral), Impulse]
    [Description("Gives you a random skin you don't have active if the current car has multiple skins")]
    public class RandomSkin : ChaosEffect
    {
        protected override void Enable()
        {
            var skin = car.GetComponent<CarSkin>();
            var current = GameState.Instance.skin;
            var rand = Random.Range(0, skin.skinsToChange.Length - 1);
            if (current <= rand) rand++;
            GameState.Instance.skin = rand;
            skin.SetSkin(rand);
        }

        public static bool Valid() => car.TryGetComponent<CarSkin>(out var skin) && skin.skinsToChange.Length > 1;
    }
}