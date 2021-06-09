using System;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class EffectGroupAttribute : Attribute
{
    private readonly string id;
    private readonly string name;
    public EffectGroupAttribute(string id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public string Id => id;
    public string Name => name;
    public EffectInfo.Alignment Alignment;
    public bool SeparateCheats = false;
}
