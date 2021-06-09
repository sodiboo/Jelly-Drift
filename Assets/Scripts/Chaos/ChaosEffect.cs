using UnityEngine;

public abstract class ChaosEffect : MonoBehaviour
{
    protected virtual void Awake() => enabled = false;
    public static bool HasEnemy;
    public static Car car => ShakeController.Instance.car;
    public static CarAI enemy;

    public virtual object[] CustomParameters() => null;

    protected virtual void Enable() { }
    protected virtual void Disable() { }
    private bool initialized;

    private void OnEnable()
    {
        initialized = true;
        Enable();
    }

    private void OnDisable()
    {
        if (initialized) Disable();
    }
}
