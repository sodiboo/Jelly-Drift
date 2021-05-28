using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class ChaosController : MonoBehaviour
{
    public static ChaosController Instance;
    public TextMeshProUGUI text;

    void Awake()
    {
        if (Instance != this && Instance != null) Destroy(this);
        Instance = this;
        text = GetComponent<TextMeshProUGUI>();
        gameObject.AddComponent<WorldObjects>();
        InputManager.Instance.debug += () =>
        {
            riggedEffect = effectMap[typeof(Chaos.TAS)];
        };
    }

    static Dictionary<Assembly, IEnumerable<EffectInfo>> _effects = new Dictionary<Assembly, IEnumerable<EffectInfo>>();

    static EffectInfo[] _allEffects;

    public static EffectInfo[] effects => _allEffects = _allEffects ?? _effects.Values.Aggregate((a, b) => a.Concat(b)).ToArray();
    public static Dictionary<Type, EffectInfo> effectMap = new Dictionary<Type, EffectInfo>();

    public static void LoadEffectsFrom(Assembly assembly)
    {
        if (_allEffects != null) throw new InvalidOperationException("Effects have already been used and adding assemblies now may result in faulty values.");
        if (_effects.ContainsKey(assembly)) return;
        _effects[assembly] = GetEffects(assembly);
    }
    static IEnumerable<EffectInfo> GetEffects(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsClass && typeof(ChaosEffect).IsAssignableFrom(type))
            {
                var attribute = type.GetCustomAttribute<EffectAttribute>();
                if (attribute != null) yield return effectMap[type] = new EffectInfo(attribute, type);
            }
        }
    }

    public void RegisterChaos()
    {
        try
        {
            LoadEffectsFrom(Assembly.GetExecutingAssembly());
        }
        catch (InvalidOperationException) { }
        foreach (var effect in effects)
        {
            print($"\"{effect.name}\" ({effect.type.FullName})\nConflicts:\n- " +
                $"{string.Join("\n- ", effect.conflicts.Select(conflict => $"\"{conflict.name}\" ({conflict.type.FullName})").ToArray())}");
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        if (chaosCoroutine != null)
        {
            StopCoroutine(chaosCoroutine);
            chaosCoroutine = null;
        }
        if (activeEffect != null)
        {
            Destroy(activeEffect);
            activeEffect = null;
        }
    }

    public EffectInfo currentEffect { get; private set; }
    ChaosEffect activeEffect;
    public EffectInfo riggedEffect { get; set; }
    public bool runNextCycle;

    private IEnumerator Chaos()
    {
        while (runNextCycle)
        {
            print("do chaos");
            if (riggedEffect == null)
            {
                var valid = new List<EffectInfo>();
                foreach (var effect in effects)
                {
                    if (effect == currentEffect) continue;
                    if (effect.valid) valid.Add(effect);
                }
                currentEffect = valid[Random.Range(0, valid.Count)];
            }
            else
            {
                currentEffect = riggedEffect;
                riggedEffect = null;
            }

            text.text = currentEffect.name;
            activeEffect = (ChaosEffect)gameObject.AddComponent(currentEffect.type);

            yield return new WaitForSeconds(5f);

            if (activeEffect != null)
            {
                Destroy(activeEffect);
                activeEffect = null;
                text.text = "";
            }
        }

        chaosCoroutine = null;
    }

    Coroutine chaosCoroutine;

    public void StartChaos()
    {
        runNextCycle = true;
        if (chaosCoroutine == null) chaosCoroutine = StartCoroutine(Chaos());
        print("start chaos");
    }

    public void StopChaos()
    {
        runNextCycle = false;
    }
}
