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
    #region Reflection

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

    #endregion

    #region Lifecycle

    public static ChaosController Instance;
    public TextMeshProUGUI text;

    void Awake()
    {
        if (Instance != this && Instance != null) Destroy(this);
        Instance = this;
        text = GetComponent<TextMeshProUGUI>();
        gameObject.AddComponent<WorldObjects>();
        InputManager.Instance.debug += EnableCheats;
    }

    public void RegisterChaos()
    {
        try
        {
            LoadEffectsFrom(Assembly.GetExecutingAssembly());
        }
        catch (InvalidOperationException) { }
    }

    private void OnDestroy()
    {
        InputManager.Instance.debug -= EnableCheats;
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

    #endregion

    #region Chaos

    public EffectInfo currentEffect { get; private set; }
    ChaosEffect activeEffect;
    public EffectInfo riggedEffect { get; set; }
    public bool runNextCycle;

    private IEnumerator Chaos()
    {
        while (runNextCycle)
        {
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
        if (cheatMode == true) useCheats = true;
    }

    Coroutine chaosCoroutine;

    public void StartChaos()
    {
        if (cheatMode) return;
        runNextCycle = true;
        if (chaosCoroutine == null) chaosCoroutine = StartCoroutine(Chaos());
    }

    public void StopChaos()
    {
        runNextCycle = false;
    }

    #endregion

    #region Cheats

    public float cheatSpacing = 6f;
    public float cheatPadding = 6f;

    void EnableCheats()
    {
        if (!cheatMode)
        {
            foreach (var effect in effects) if (!effect.noCheat) cheatEffectsList.Add(effect);
            cheatEffectsList.Sort((a, b) =>
            {
                if (a.impulse != b.impulse)
                {
                    return a.impulse ? -1 : 1;
                }
                return StringComparer.OrdinalIgnoreCase.Compare(a.name, b.name);
            });
            CalculateSpacing();
            text.alpha = 0f;
        }
        if (chaosCoroutine != null)
        {
            StopChaos();
            cheatMode = true;
        }
        else
        {
            cheatMode = true;
            useCheats = !useCheats;
        }
    }

    bool cheatMode;
    bool useCheats;
    Vector2 scrollPosition;

    Dictionary<EffectInfo, ChaosEffect> cheatEffects = new Dictionary<EffectInfo, ChaosEffect>();
    List<EffectInfo> cheatEffectsList = new List<EffectInfo>();
    float width;
    float height;
    float totalHeight;

    private void CalculateSpacing()
    {
        var maxSize = Vector2.zero;
        foreach (var effect in effects)
        {
            maxSize = Vector2.Max(maxSize, GUIStyle.none.CalcSize(new GUIContent(effect.name)));
        }
        totalHeight = (maxSize.y + cheatSpacing + cheatPadding) * cheatEffectsList.Count - cheatSpacing;
        height = maxSize.y + cheatPadding;
        width = maxSize.x + height; // checkbox makes it slightly wider
    }

    private void OnGUI()
    {
        if (useCheats && !Pause.Instance.paused)
        {
#if UNITY_EDITOR
            CalculateSpacing(); // update spacing in realtime in editor, no need in release for performance reasons
#endif
            scrollPosition = GUI.BeginScrollView(new Rect(10, 10, Mathf.Min(width, Screen.width - 40) + 20, Mathf.Min(totalHeight, Screen.height - 40) + 20), scrollPosition, new Rect(0, 0, width, totalHeight));

            for (var i = 0; i < cheatEffectsList.Count; i++)
            {
                var effect = cheatEffectsList[i];
                if (effect.conflicts.Any(cheatEffects.ContainsKey)) continue;
                var y = (height + cheatSpacing) * i;
                if (effect.impulse)
                {
                    if (GUI.Button(new Rect(0, y, width, height), effect.name))
                    {
                        Destroy(gameObject.AddComponent(effect.type), 0.5f);
                    }
                }
                else
                {
                    if (GUI.Toggle(new Rect(0, y, width, height), cheatEffects.ContainsKey(effect), effect.name))
                    {
                        if (!cheatEffects.ContainsKey(effect))
                        {
                            foreach (var conflict in effect.conflicts)
                            {
                                if (cheatEffects.TryGetValue(conflict, out var component))
                                {
                                    Destroy(component);
                                    cheatEffects.Remove(conflict);
                                }
                            }
                            cheatEffects[effect] = (ChaosEffect)gameObject.AddComponent(effect.type);
                        }
                    }
                    else
                    {
                        if (cheatEffects.ContainsKey(effect))
                        {
                            Destroy(cheatEffects[effect]);
                            cheatEffects.Remove(effect);
                        }
                    }
                }
            }

            GUI.EndScrollView();
        }
    }

    #endregion
}
