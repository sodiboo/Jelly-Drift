using System;

[AttributeUsage(AttributeTargets.Class)]
public class EffectAttribute : Attribute
{
    private readonly string name;
    private readonly string id;
    private readonly EffectInfo.Alignment alignment;
    public EffectAttribute(string id, string name, EffectInfo.Alignment alignment)
    {
        this.name = name;
        this.id = id;
        this.alignment = alignment;
    }
    public string Name => name;
    public string Id => id;
    public EffectInfo.Alignment Alignment => alignment;

}
