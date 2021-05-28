using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChaosEffect : MonoBehaviour
{
    public static bool HasEnemy;
    public string Name { get; }
    public static Car car => ShakeController.Instance.car;
    public static CarAI enemy;
}
