using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using RapidGUI;

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
                var effect = type.GetCustomAttribute<EffectAttribute>();
                if (effect != null)
                {
                    yield return effectMap[type] = new EffectInfo(effect, type);
                    continue;
                }

                var group = type.GetCustomAttribute<EffectGroupAttribute>();
                if (group != null)
                {
                    yield return effectMap[type] = new EffectInfo(group, type);
                    continue;
                }

                var child = type.GetCustomAttribute<ChildEffectAttribute>();
                if (child != null)
                {
                    var info = new EffectInfo(child, type);
                    yield return effectMap[type] = info;
                    continue;
                }
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
        globalFormatting.Add("map", () => MapManager.Instance.maps[GameState.Instance.map].name);
        globalFormatting.Add("car", () => PrefabManager.Instance.cars[GameState.Instance.car].name);
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
            effect.Setup();
        }
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
    List<ChaosEffect> activeChildren;
    ChaosEffect activeEffect;
    public EffectInfo riggedEffect { get; set; }
    public bool runNextCycle;

    public Dictionary<string, Func<string>> globalFormatting = new Dictionary<string, Func<string>>();

    private IEnumerator Chaos()
    {
        while (runNextCycle)
        {
            if (riggedEffect == null)
            {
                var valid = new List<EffectInfo>();
                foreach (var effect in effects)
                {
                    if (effect.isChild) continue;
                    if (effect == currentEffect || effect == currentEffect?.parent) continue;
                    if (effect.valid)
                    {
                        valid.Add(effect);
                    }
                }
                currentEffect = valid[Random.Range(0, valid.Count)];
            }
            else
            {
                currentEffect = riggedEffect;
                riggedEffect = null;
            }

            if (currentEffect.effectType == EffectInfo.EffectType.ExclusiveGroup)
            {
                currentEffect = currentEffect.children[Random.Range(0, currentEffect.children.Length)];
            }

            text.text = globalFormatting.Aggregate(currentEffect.name, (name, format) => name.Replace($"@{format.Key}", format.Value()));

            if (currentEffect.effectType == EffectInfo.EffectType.MultiGroup)
            {
                activeChildren = new List<ChaosEffect>();
                foreach (var child in currentEffect.children)
                {
                    if (child.valid) activeChildren.Add((ChaosEffect)gameObject.AddComponent(child.type));
                }
            }
            else
            {
                if (currentEffect.isChild || currentEffect.isGroup) Debug.LogWarning($"{currentEffect.id} is not independent but should be");
                activeEffect = (ChaosEffect)gameObject.AddComponent(currentEffect.type);
            }
            var effectSpecificParams = activeEffect?.CustomParameters();
            if (effectSpecificParams != null) text.text = string.Format(text.text, effectSpecificParams);

            yield return new WaitForSeconds(5f);

            if (activeEffect != null)
            {
                Destroy(activeEffect);
                activeEffect = null;
            }
            if (activeChildren != null)
            {
                foreach (var child in activeChildren) Destroy(child);
                activeChildren = null;
            }
            text.text = "";
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
        if (!cheatMode) InitCheats();
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

    Dictionary<EffectInfo, ChaosEffect> cheatEffects = new Dictionary<EffectInfo, ChaosEffect>();
    List<EffectInfo> cheatEffectsList = new List<EffectInfo>();

    Dictionary<EffectInfo, ChaosEffect> activeCheats;

    private void RemoveEffect(EffectInfo info)
    {
        if (!activeCheats.TryGetValue(info, out var effect)) return;

        foreach (var toReload in info.reloadOnDisable)
        {
            if (activeCheats.TryGetValue(toReload, out var reload)) reload.enabled = false;
        }

        Destroy(effect);
        activeCheats.Remove(info);

        foreach (var toReload in info.reloadOnDisable)
        {
            if (activeCheats.TryGetValue(toReload, out var reload)) reload.enabled = true;
        }
    }

    private void AddEffect(EffectInfo info)
    {
        if (activeCheats.ContainsKey(info)) return;
        foreach (var toReload in info.reloadOnEnable)
        {
            if (activeCheats.TryGetValue(toReload, out var reload)) reload.enabled = false;
        }

        activeCheats[info] = (ChaosEffect)gameObject.AddComponent(info.type);

        foreach (var toReload in info.reloadOnEnable)
        {
            if (activeCheats.TryGetValue(toReload, out var reload)) reload.enabled = true;
        }
    }

    float maxWidth = 0f;
    float indent = 32f;

    private void InitCheats()
    {
        windowRect = new Rect(20, 20, 300, Screen.height - 40);
        activeCheats = new Dictionary<EffectInfo, ChaosEffect>();
        foreach (var effect in effects)
        {
            if (!effect.noCheat && !effect.isChild)
            {
                if (effect.splitCheats)
                {
                    foreach (var child in effect.children) if (!child.noCheat) cheatEffectsList.Add(child);
                }
                else cheatEffectsList.Add(effect);
            }
        }

        cheatEffectsList.Sort((a, b) =>
            {
                if (a.impulse != b.impulse)
                {
                    return a.impulse ? -1 : 1;
                }
                if (a.isGroup != b.isGroup)
                {
                    return a.isGroup ? -1 : 1;
                }
                return StringComparer.OrdinalIgnoreCase.Compare(a.name, b.name);
            });
        text.alpha = 0f;
    }

    Rect windowRect;
    Vector2 scrollPos;

    private string DisplayName(EffectInfo effect)
    {
        if (effect.valid) return effect.name;
        return $"{effect.name} (!)";
    }

    private void OnGUI()
    {
        if (!useCheats || Pause.Instance.paused || !GameController.Instance.playing) return;
        if (maxWidth == 0f)
        {
            foreach (var effect in cheatEffectsList)
            {
                if (!effect.isGroup)
                {
                    maxWidth = Mathf.Max(GUI.skin.toggle.CalcSize(new GUIContent(effect.name)).x + GUI.skin.toggle.fontSize, maxWidth);
                }
                else if (effect.effectType == EffectInfo.EffectType.MultiGroup)
                {
                    maxWidth = Mathf.Max(GUI.skin.toggle.CalcSize(new GUIContent(effect.name)).x + GUI.skin.toggle.fontSize, maxWidth);
                    foreach (var child in effect.children)
                    {
                        maxWidth = Mathf.Max(GUI.skin.toggle.CalcSize(new GUIContent(effect.name)).x + GUI.skin.toggle.fontSize + indent, maxWidth);
                    }
                }
                else
                {
                    maxWidth = Mathf.Max(GUI.skin.label.CalcSize(new GUIContent(effect.name)).x, maxWidth);
                    foreach (var child in effect.children)
                    {
                        maxWidth = Mathf.Max(GUI.skin.toggle.CalcSize(new GUIContent(effect.name)).x + GUI.skin.toggle.fontSize + indent, maxWidth);
                    }
                }
            }
        }


        windowRect = RGUI.ResizableWindow(GetHashCode(), windowRect, _ =>
        {
            GUI.DragWindow(new Rect(0, 0, windowRect.width, 20));
            using (var scrollView = new GUILayout.ScrollViewScope(scrollPos, GUIStyle.none, GUIStyle.none))
            {
                scrollPos = scrollView.scrollPosition;
                foreach (var effect in cheatEffectsList)
                {
                    if (effect.isGroup)
                    {
                        using (new RGUI.EnabledScope(!effect.children.All(child => child.conflicts.Any(activeCheats.ContainsKey))))
                        {
                            if (effect.effectType == EffectInfo.EffectType.MultiGroup)
                            {
                                var all = effect.children.Any(activeCheats.ContainsKey);
                                if (all != GUILayout.Toggle(all, effect.name))
                                {
                                    if (all)
                                    {
                                        foreach (var child in effect.children) RemoveEffect(child);
                                    }
                                    else
                                    {
                                        foreach (var child in effect.children)
                                        {
                                            if (!child.conflicts.Any(activeCheats.ContainsKey) && child.valid) AddEffect(child);
                                        }
                                    }
                                }
                            }
                            else GUILayout.Label(effect.name);
                            using (new RGUI.IndentScope(indent))
                            {

                                foreach (var child in effect.children)
                                {
                                    using (new RGUI.EnabledScope(!child.conflicts.Any(activeCheats.ContainsKey)))
                                    {
                                        var active = activeCheats.ContainsKey(child);
                                        if (active != GUILayout.Toggle(active, DisplayName(child)))
                                        {
                                            if (active)
                                            {
                                                RemoveEffect(child);
                                            }
                                            else
                                            {
                                                if (effect.effectType == EffectInfo.EffectType.ExclusiveGroup)
                                                {
                                                    foreach (var otherChild in effect.children) RemoveEffect(otherChild);
                                                }
                                                AddEffect(child);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                    else if (effect.impulse)
                    {
                        using (new RGUI.EnabledScope(!effect.conflicts.Any(activeCheats.ContainsKey)))
                        {
                            if (GUILayout.Button(DisplayName(effect)))
                            {
                                AddEffect(effect);
                            }
                            else
                            {
                                RemoveEffect(effect);
                            }
                        }
                    }
                    else
                    {
                        using (new RGUI.EnabledScope(!effect.conflicts.Any(activeCheats.ContainsKey)))
                        {
                            var active = activeCheats.ContainsKey(effect);
                            if (active != GUILayout.Toggle(active, DisplayName(effect)))
                            {
                                if (active)
                                {
                                    RemoveEffect(effect);
                                }
                                else
                                {
                                    AddEffect(effect);
                                }
                            }
                        }
                    }
                }
            }
        }, "Chaos Cheats", null, GUILayout.MinWidth(maxWidth + GUI.skin.window.padding.horizontal * 2));
    }

    #endregion
}
