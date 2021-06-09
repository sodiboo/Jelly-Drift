using System.Collections.Generic;
using UnityEngine;

namespace MilkShake
{
    // Token: 0x0200005D RID: 93
    [AddComponentMenu("MilkShake/Shaker")]
    public class Shaker : MonoBehaviour
    {
        // Token: 0x0600021A RID: 538 RVA: 0x0000A53E File Offset: 0x0000873E
        public static ShakeInstance ShakeAll(IShakeParameters shakeData, int? seed = null)
        {
            var shakeInstance = new ShakeInstance(shakeData, seed);
            Shaker.AddShakeAll(shakeInstance);
            return shakeInstance;
        }

        // Token: 0x0600021B RID: 539 RVA: 0x0000A550 File Offset: 0x00008750
        public static void ShakeAllSeparate(IShakeParameters shakeData, List<ShakeInstance> shakeInstances = null, int? seed = null)
        {
            if (shakeInstances != null)
            {
                shakeInstances.Clear();
            }
            for (var i = 0; i < Shaker.GlobalShakers.Count; i++)
            {
                if (Shaker.GlobalShakers[i].gameObject.activeInHierarchy)
                {
                    var shakeInstance = Shaker.GlobalShakers[i].Shake(shakeData, seed);
                    if (shakeInstances != null && shakeInstance != null)
                    {
                        shakeInstances.Add(shakeInstance);
                    }
                }
            }
        }

        // Token: 0x0600021C RID: 540 RVA: 0x0000A5B4 File Offset: 0x000087B4
        public static void ShakeAllFromPoint(Vector3 point, float maxDistance, IShakeParameters shakeData, List<ShakeInstance> shakeInstances = null, int? seed = null)
        {
            if (shakeInstances != null)
            {
                shakeInstances.Clear();
            }
            for (var i = 0; i < Shaker.GlobalShakers.Count; i++)
            {
                if (Shaker.GlobalShakers[i].gameObject.activeInHierarchy)
                {
                    var shakeInstance = Shaker.GlobalShakers[i].ShakeFromPoint(point, maxDistance, shakeData, seed);
                    if (shakeInstances != null && shakeInstance != null)
                    {
                        shakeInstances.Add(shakeInstance);
                    }
                }
            }
        }

        // Token: 0x0600021D RID: 541 RVA: 0x0000A61C File Offset: 0x0000881C
        public static void AddShakeAll(ShakeInstance shakeInstance)
        {
            for (var i = 0; i < Shaker.GlobalShakers.Count; i++)
            {
                if (Shaker.GlobalShakers[i].gameObject.activeInHierarchy)
                {
                    Shaker.GlobalShakers[i].AddShake(shakeInstance);
                }
            }
        }

        // Token: 0x0600021E RID: 542 RVA: 0x0000A666 File Offset: 0x00008866
        private void Awake()
        {
            if (addToGlobalShakers)
            {
                Shaker.GlobalShakers.Add(this);
            }
        }

        // Token: 0x0600021F RID: 543 RVA: 0x0000A67B File Offset: 0x0000887B
        private void OnDestroy()
        {
            if (addToGlobalShakers)
            {
                Shaker.GlobalShakers.Remove(this);
            }
        }

        // Token: 0x06000220 RID: 544 RVA: 0x0000A694 File Offset: 0x00008894
        private void Update()
        {
            if (SaveState.Instance.cameraShake == 0)
            {
                return;
            }
            var shakeResult = default(ShakeResult);
            for (var i = 0; i < activeShakes.Count; i++)
            {
                if (activeShakes[i].IsFinished)
                {
                    activeShakes.RemoveAt(i);
                    i--;
                }
                else
                {
                    shakeResult += activeShakes[i].UpdateShake(Time.deltaTime);
                }
            }
            base.transform.localPosition = shakeResult.PositionShake;
            base.transform.localEulerAngles = shakeResult.RotationShake;
        }

        // Token: 0x06000221 RID: 545 RVA: 0x0000A730 File Offset: 0x00008930
        public ShakeInstance Shake(IShakeParameters shakeData, int? seed = null)
        {
            var shakeInstance = new ShakeInstance(shakeData, seed);
            AddShake(shakeInstance);
            return shakeInstance;
        }

        // Token: 0x06000222 RID: 546 RVA: 0x0000A750 File Offset: 0x00008950
        public ShakeInstance ShakeFromPoint(Vector3 point, float maxDistance, IShakeParameters shakeData, int? seed = null)
        {
            var num = Vector3.Distance(base.transform.position, point);
            if (num < maxDistance)
            {
                var shakeInstance = new ShakeInstance(shakeData, seed);
                var num2 = 1f - Mathf.Clamp01(num / maxDistance);
                shakeInstance.StrengthScale = num2;
                shakeInstance.RoughnessScale = num2;
                AddShake(shakeInstance);
                return shakeInstance;
            }
            return null;
        }

        // Token: 0x06000223 RID: 547 RVA: 0x0000A7A3 File Offset: 0x000089A3
        public void AddShake(ShakeInstance shakeInstance) => activeShakes.Add(shakeInstance);

        // Token: 0x0400022F RID: 559
        public static List<Shaker> GlobalShakers = new List<Shaker>();

        // Token: 0x04000230 RID: 560
        [SerializeField]
        private bool addToGlobalShakers;

        // Token: 0x04000231 RID: 561
        private readonly List<ShakeInstance> activeShakes = new List<ShakeInstance>();
    }
}
