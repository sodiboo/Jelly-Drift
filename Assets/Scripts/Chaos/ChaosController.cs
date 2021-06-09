using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        InputManager.Instance.debug += EnableCheats; // () => riggedEffect = effectMap[typeof(Chaos.TaskEffect)];
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

        foreach (var effect in effects)
        {
            if (effect.effectType == EffectInfo.EffectType.MultiGroup && effect.alignment == default) print($"{effect.id} has no alignment");
        }
#if !MOBILE
        var path = Path.Combine(Application.persistentDataPath, "effects.json");
        ChaosConfig config;
        if (File.Exists(path))
        {
            var serializedEffects = new List<ChaosConfig.SerializedEffect>();
            foreach (var effect in effects)
            {
                if (effect.id == "null") continue;
                serializedEffects.Add(new ChaosConfig.SerializedEffect(effect));
            }

            try
            {

                using (var file = File.Open(path, FileMode.OpenOrCreate))
                {
                    using (var reader = new StreamReader(file)) config = JsonUtility.FromJson<ChaosConfig>(reader.ReadToEnd());
                    if (config.config != null)
                        foreach (var configured in config.config)
                        {
                            try
                            {
                                var effect = effects.First(e => e.id == configured.id);
                                effect.name = configured.name;
                                effect.duration = configured.duration;
                            }
                            catch (InvalidOperationException) { } // no effect has that id
                        }
                }
            }
            catch (Exception)
            {
                config = new ChaosConfig();
            }
            config.effects = serializedEffects.ToArray();

            using (var file = File.Open(path, FileMode.Truncate))
            using (var writer = new StreamWriter(file)) writer.Write(JsonUtility.ToJson(config));
        }
    }

    [Serializable]
    private class ChaosConfig
    {
        public SerializedEffect[] effects;
        public EffectConfig[] config;

        [Serializable]
        public class SerializedEffect
        {
            public SerializedEffect(EffectInfo effect)
            {
                id = effect.id;
                name = effect.name;
                parent = effect.parent?.id;
                description = effect.description;
                conflicts = effect.conflicts.Select(conflict => conflict.id).ToArray();
                children = effect.children?.Select(child => child.id).ToArray();
                type = (int)effect.effectType;
                splitCheats = effect.splitCheats;
                noCheat = effect.noCheat;
                impulse = effect.impulse;
            }
            public string id;
            public string name;
            public string parent;
            public string description;
            public string[] conflicts;
            public string[] children;
            public int type;
            public bool splitCheats;
            public bool noCheat;
            public bool impulse;
        }

        [Serializable]
        public class EffectConfig
        {
            public string id;
            public string name;
            public float duration;
        }
#endif
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

    EffectInfo currentEffect;
    List<ChaosEffect> activeChildren;
    ChaosEffect activeEffect;
    public EffectInfo riggedEffect { get; set; }
    public bool runNextCycle;

    public Dictionary<string, Func<string>> globalFormatting = new Dictionary<string, Func<string>>();

    private IEnumerator Chaos()
    {
        while (runNextCycle)
        {
            var alignment = default(EffectInfo.Alignment);
            if (riggedEffect == null)
            {
                alignment = (EffectInfo.Alignment)Random.Range(1, 4);
                var valid = new List<EffectInfo>();
                foreach (var effect in effects)
                {
                    if (effect.isChild) continue;
                    if (effect == currentEffect || effect == currentEffect?.parent) continue;
                    if (effect.effectType == EffectInfo.EffectType.ExclusiveGroup)
                    {
                        if (!effect.children.Any(child => child.alignment == alignment && child.duration > 0f)) continue;
                    }
                    if (effect.valid && effect.duration > 0f)
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
                if (alignment == default)
                {
                    currentEffect = currentEffect.children[Random.Range(0, currentEffect.children.Length)];
                }
                else
                {
                    var correctAlignment = currentEffect.children.Where(child => child.alignment == alignment && child.duration > 0f).ToList();
                    currentEffect = correctAlignment[Random.Range(0, correctAlignment.Count)];
                }
            }


            if (currentEffect.effectType == EffectInfo.EffectType.MultiGroup)
            {
                activeChildren = new List<ChaosEffect>();
                foreach (var child in currentEffect.children)
                {
                    if (child.valid)
                    {
                        var effect = (ChaosEffect)gameObject.AddComponent(child.type);
                        activeChildren.Add(effect);
                        effect.enabled = true;
                    }
                }
            }
            else
            {
                activeEffect = (ChaosEffect)gameObject.AddComponent(currentEffect.type);
                activeEffect.enabled = true;
            }


        wait:

            yield return new WaitForSeconds(currentEffect.duration);

            if (activeEffect is Chaos.TaskEffect)
            {
                var task = activeEffect as Chaos.TaskEffect;
                text.text = task.CheckTask(out currentEffect, out activeEffect, out activeChildren);
                Destroy(task);
                goto wait;
            }

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
        }

        currentEffect = null;
        chaosCoroutine = null;
        if (cheatMode == true) useCheats = true;
    }

    private void Update()
    {
        if (currentEffect == null)
        {
            text.text = "";
            return;
        }
        text.text = globalFormatting.Aggregate(currentEffect.name, (name, format) => name.Replace($"@{format.Key}", format.Value()));
        var effectSpecificParams = activeEffect?.CustomParameters();
        if (effectSpecificParams != null) text.text = string.Format(text.text, effectSpecificParams);
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
        activeCheats[info].enabled = true;

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
