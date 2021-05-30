using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class EffectInfo
{

    public readonly string name;
    public readonly string id;
    public readonly Type type;
    public EffectInfo[] conflicts => _computedConflicts = _computedConflicts ?? ComputeConflicts().ToArray();
    public bool valid => (bool) (_valid?.Invoke(null, null) ?? true);
    public bool impulse;
    public bool noCheat;
    public EffectInfo(EffectAttribute attribute, Type type)
    {
        id = attribute.Id;
        this.type = type;
        _conflicts = Type.EmptyTypes;
        _computedConflicts = null;

        name = attribute.Name ?? type.FullName;
        _valid = type.GetMethod("Valid", BindingFlags.Static | BindingFlags.Public, null, Type.EmptyTypes, null);
        var conflicts = type.GetCustomAttribute<ConflictsWithAttribute>();
        if (conflicts != null) _conflicts = conflicts.Conflicts;
        impulse = type.GetCustomAttribute<ImpulseAttribute>() != null;
        noCheat = type.GetCustomAttribute<HideInCheatGUIAttribute>() != null;
    }

    private readonly MethodInfo _valid;
    private readonly Type[] _conflicts;
    private EffectInfo[] _computedConflicts;

    private IEnumerable<EffectInfo> ComputeConflicts()
    {
        foreach (var t in _conflicts)
        {
            foreach (var effect in ChaosController.effects)
            {
                var thisType = type; // argh, can't use this in lambda for checking cross-conflicts
                if (effect.type == thisType) continue;
                if (t.IsAssignableFrom(effect.type) || effect._conflicts.Any(type => type.IsAssignableFrom(thisType))) yield return effect;
            }
        }
    }
}
